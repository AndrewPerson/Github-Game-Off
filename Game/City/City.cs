using Godot;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Game;

class City
{
    public Vector2 position;
    public List<Connection> connections = new();
    public Dictionary<Cliche, ClicheCityStats> clicheStats = new();
    public Cliche? producedCliche = null;

    public City(Vector2 position)
    {
        this.position = position;
    }

    public void ConnectTo(City other)
    {
        var newConnection = new Connection(this, other);

        connections.Add(newConnection);
        other.connections.Add(newConnection);
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
        connections.RemoveAll(x => x.to == other);
        other.connections.RemoveAll(x => x.from == this);
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

    public void ReceiveCliche(Cliche cliche, float catchiness)
    {
        if (clicheStats.ContainsKey(cliche))
        {
            clicheStats[cliche].catchiness += catchiness;
        }
        else
        {
            clicheStats.Add(cliche, new ClicheCityStats(catchiness, 0));
        }
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