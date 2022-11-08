using System;

namespace Game;

enum Direction
{
    FROM,
    TO
}

class Connection
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
        if (from.producedCliche == null)
            return;

        //A catchiness falloff multiplier of 0.5 should result in the penalty being 50% less harsh.
        //That means the graph of length vs penalty should be stretched by 50%.
        float penalty = MathF.Pow(Length, CATCHINESS_LENGTH_FALLOFF);

        to.ReceiveCliche(from.producedCliche, from.producedCliche.GetCatchiness(to) * penalty);
    }
}

class RelativeConnection
{
    public Direction direction;
    public City other;

    public RelativeConnection(Direction direction, City other)
    {
        this.other = other;
        this.direction = direction;
    }
}