using Godot;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Game;

struct City
{
    public Vector2 position;
    public List<Connection> connections;
    public List<ClicheSpread> clicheSpreads;
    public Cliche? producedCliche;

    public City(Vector2 position)
    {
        this.position = position;
        connections = new List<Connection>();
        clicheSpreads = new List<ClicheSpread>();
        producedCliche = null;
    }

    public void Spread()
    {
        if (producedCliche == null)
            return;

        foreach (var connection in connections)
        {
            var relativeConnection = connection.GetRelativeConnection(this);
            if (relativeConnection.direction == Direction.FROM)
            {
                throw new System.NotImplementedException("Spreading is not implemented yet");
            }
        }
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj.GetType() == typeof(City))
        {
            City other = (City)obj;
            return position == other.position && connections == other.connections;
        }
        else return false;
    }

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(City lhs, City rhs)
    {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(City lhs, City rhs)
    {
        return !lhs.Equals(rhs);
    }
}

struct ClicheSpread
{
    public Cliche cliche;
    public float percentageSpread;

    public ClicheSpread(Cliche cliche, float percentageSpread)
    {
        this.cliche = cliche;
        this.percentageSpread = percentageSpread;
    }
}

enum Direction
{
    FROM,
    TO
}

struct Connection
{
    public City from;
    public City to;
    public float Length => (to.position - from.position).Length();

    public RelativeConnection GetRelativeConnection(City node)
    {
        if (node == from)
        {
            return new RelativeConnection(Direction.FROM, to);
        }
        else if (node == to)
        {
            return new RelativeConnection(Direction.TO, from);
        }
        else
        {
            throw new System.Exception("City is not connected to this connection.");
        }
    }
}

struct RelativeConnection
{
    public Direction direction;
    public City other;

    public RelativeConnection(Direction direction, City other)
    {
        this.other = other;
        this.direction = direction;
    }
}