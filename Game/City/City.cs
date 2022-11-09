using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Game;

public class City
{
    public event Action? OnUpdateConnections;
    public event Action? OnUpdateCliches;
    public event Action? OnUpdateProducedCliche;

    public string name;
    public Vector2 position;
    public List<Connection> connections = new();
    public Dictionary<Cliche, ClicheCityStats> clicheStats = new();
    public Cliche? producedCliche = null;    

    public City(string name, Vector2 position)
    {
        this.name = name;
        this.position = position;
    }

    public void CalculateInternalClicheSpreads()
    {
        var sortedCliches = clicheStats.OrderBy(s => s.Value.catchiness).ToArray();
        
        var summedCatchiness = 0f;
        for (int i = 0; i < sortedCliches.Length; i++)
        {
            var (cliche, stats) = sortedCliches[i];

            if (i > 0)
            {
                //TODO Tweak the dynamic spread percentage
                var spread = 0.02f * stats.spreadPercentage;

                for (int x = 0; x < i; x++)
                {
                    var spreadLoss = 1 - sortedCliches[x].Value.catchiness / summedCatchiness;
                    spreadLoss *= spread;

                    //TODO Remove cliches when their percentage drops to zero.
                    sortedCliches[x].Value.spreadPercentage -= spreadLoss;
                }

                //TODO Cap the spread at 1. (100%)
                sortedCliches[i].Value.spreadPercentage += spread;
            }

            summedCatchiness += stats.catchiness;
        }

        foreach (var (cliche, stats) in sortedCliches)
        {
            clicheStats[cliche] = stats;
        }

        OnUpdateCliches?.Invoke();
    }

    public void ConnectTo(City other)
    {
        var newConnection = new Connection(this, other);

        connections.Add(newConnection);
        other.connections.Add(newConnection);

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

        if (obj.GetType() == typeof(City))
        {
            City other = (City)obj;
            return position == other.position && connections == other.connections;
        }
        else return false;
    }

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(City? lhs, City? rhs)
    {
        if (lhs == null) return rhs == null;
        if (rhs == null) return lhs == null;

        return lhs.Equals(rhs);
    }

    public static bool operator !=(City? lhs, City? rhs)
    {
        return !(lhs == rhs);
    }
}