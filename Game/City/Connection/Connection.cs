using System;

namespace Game;

public enum Direction
{
    FROM,
    TO
}

public class Connection
{
    //At 7 units of distance, the cliche will only be half as catchy.
    //CATCHINESS_LENGTH_FALLOFF = 0.5^(1/7)
    public static float CATCHINESS_LENGTH_FALLOFF = 0.906f;

    public City from;
    public City to;
    public float Length => (to.position - from.position).Length();

    public Connection(City from, City to)
    {
        this.from = from;
        this.to = to;
    }

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

    public void SpreadProducedCliche()
    {
        if (from.ProducedCliche == null)
            return;

        float penalty = MathF.Pow(Length, CATCHINESS_LENGTH_FALLOFF);

        to.AddCliche(from.ProducedCliche, from.ProducedCliche.GetCatchiness(to) * penalty);
    }

    public void RemoveProducedCliche()
    {
        if (from.ProducedCliche == null)
            return;

        float penalty = MathF.Pow(Length, CATCHINESS_LENGTH_FALLOFF);

        to.RemoveCliche(from.ProducedCliche, from.ProducedCliche.GetCatchiness(to) * penalty);
    }

    public void Disconnect()
    {
        from.DisconnectFrom(to);
    }
}

public class RelativeConnection
{
    public Direction direction;
    public City other;

    public RelativeConnection(Direction direction, City other)
    {
        this.other = other;
        this.direction = direction;
    }
}