using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Proxem.NumNet;

namespace Game;

public class City
{
    public event Action? OnUpdateConnections;
    public event Action? OnUpdateCliches;
    public event Action? OnUpdateProducedCliche;

    public string name;
    public Vector2 position;
    public List<Array<float>> likeVectors;
    public List<Connection> connections = new();
    public Dictionary<Cliche, ClicheCityStats> clicheStats = new();

    public Player? ControlledBy
    {
        get
        {
            if (clicheStats.Count == 0) return null;

            Dictionary<Player, float> playerPercentages = clicheStats.Aggregate(new Dictionary<Player, float>(), (acc, cliche) =>
            {
                if (!acc.ContainsKey(cliche.Key.player)) acc[cliche.Key.player] = 0;
                acc[cliche.Key.player] += cliche.Value.spread;
                return acc;
            });

            var (maxPlayer, maxPercentage) = playerPercentages.Aggregate((acc, player) => player.Value > acc.Value ? player : acc);

            if (maxPercentage < 0.5) return null;
            else return maxPlayer;
        }
    }

    private Cliche? producedCliche;
    public Cliche? ProducedCliche
    {
        get
        {
            if (producedCliche != null && producedCliche.player != ControlledBy)
            {
                producedCliche = null;
            }

            return producedCliche;
        }

        set
        {
            producedCliche = value;
            OnUpdateProducedCliche?.Invoke();
        }
    }

    public City(string name, Vector2 position, List<Array<float>> likeVectors)
    {
        this.name = name;
        this.position = position;
        this.likeVectors = likeVectors;
    }

    public void CalculateInternalClicheSpreads()
    {
        var sortedCliches = clicheStats.OrderBy(s => s.Value.catchiness).ToArray();

        var summedCatchiness = 0f;
        var catchinessCount = 0;
        var freeSpace = 1 - clicheStats.Sum(x => x.Value.spread);

        for (int i = 0; i < sortedCliches.Length; i++)
        {
            var (cliche, stats) = sortedCliches[i];

            //TODO Tweak the dynamic spread percentage
            var spread = 0.02f * MathF.Max(stats.spread, 0.1f);
            var actualSpread = 0f;

            var avgCatchiness = summedCatchiness / catchinessCount;

            foreach (var (_, stats2) in sortedCliches[..i])
            {
                var spreadLoss = avgCatchiness - (stats2.catchiness - avgCatchiness);
                spreadLoss *= 1 / summedCatchiness;
                spreadLoss *= spread;

                if (spreadLoss >= stats2.spread)
                {
                    actualSpread += stats2.spread;
                    stats2.spread = 0;

                    summedCatchiness -= stats2.catchiness;
                    catchinessCount--;
                }
                else
                {
                    actualSpread += spreadLoss;

                    stats2.spread -= spreadLoss;
                }
            }

            var usedSpace = MathF.Min(freeSpace, spread);
            actualSpread += usedSpace;
            freeSpace -= usedSpace;

            sortedCliches[i].Value.spread += actualSpread;

            summedCatchiness += stats.catchiness;
            catchinessCount++;
        }

        foreach (var (cliche, stats) in sortedCliches)
        {
            if (stats.spread <= 0)
            {
                clicheStats.Remove(cliche);
            }
        }

        //To compensate for precision errors.
        if (clicheStats.Count == 1)
        {
            clicheStats[clicheStats.Keys.First()].spread = 1;
        }

        OnUpdateCliches?.Invoke();
    }

    public void ConnectTo(City other)
    {
        var newConnection = new Connection(this, other);

        connections.Add(newConnection);
        newConnection.SpreadProducedCliche();

        OnUpdateConnections?.Invoke();
    }

    /// <summary>
    /// This will only remove all connections *from* this city to the <paramref name="other">other city</paramref>
    /// If you want to remove all connections between both cities do this:
    /// <code>
    /// a.DisconnectFrom(b);
    ///
    /// b.DisconnectFrom(a);
    /// </code>
    /// </summary>
    public void DisconnectFrom(City other)
    {
        var index = connections.FindIndex(x => x.to == other);

        if (index != -1)
        {
            connections[index].RemoveProducedCliche();
            connections.RemoveAt(index);

            OnUpdateConnections?.Invoke();
        }
    }

    public void SpreadProducedCliche()
    {
        if (producedCliche == null)
            return;

        foreach (var connection in connections)
        {
            var relativeConnection = connection.GetRelativeConnection(this);
            if (relativeConnection.direction == Direction.FROM)
            {
                connection.SpreadProducedCliche();
            }
        }
    }

    public void AddCliche(Cliche cliche, float catchiness)
    {
        if (clicheStats.ContainsKey(cliche))
        {
            clicheStats[cliche].catchiness += catchiness;
        }
        else
        {
            clicheStats.Add(cliche, new ClicheCityStats(catchiness, 0));
        }

        OnUpdateCliches?.Invoke();
    }

    public void RemoveCliche(Cliche cliche, float catchiness)
    {
        if (clicheStats.ContainsKey(cliche))
        {
            clicheStats[cliche].catchiness -= catchiness;
        }

        OnUpdateCliches?.Invoke();
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj == null) return false;

        if (obj is City city2)
        {
            return position == city2.position && connections == city2.connections;
        }
        else return false;
    }

    public override int GetHashCode() => HashCode.Combine(position, connections);

    public static bool operator ==(City? lhs, City? rhs)
    {
        if (lhs is null) return rhs is null;
        if (rhs is null) return lhs is null;

        return lhs.Equals(rhs);
    }

    public static bool operator !=(City? lhs, City? rhs)
    {
        return !(lhs == rhs);
    }
}