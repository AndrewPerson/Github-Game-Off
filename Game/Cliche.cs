using Godot;
using System.Diagnostics.CodeAnalysis;

namespace Game;

public class Cliche
{
    public readonly string text;

    public Color Colour
    {
        get
        {
            var hash = GetHashCode();

            var r = (hash & 0xFF) / 255.0f;
            r = (r + 1) / 2;

            var g = ((hash >> 8) & 0xFF) / 255.0f;
            g = (g + 1) / 2;

            var b = ((hash >> 16) & 0xFF) / 255.0f;
            b = (b + 1) / 2;

            return new Color(r, g, b);
        }
    }

    public Cliche(string text)
    {
        this.text = text;
    }

    public float GetCatchiness(City city)
    {
        throw new System.NotImplementedException("Catchiness not implemented");
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj == null) return false;

        if (obj.GetType() == typeof(Cliche))
        {
            Cliche other = (Cliche)obj;
            return other.text == text;
        }
        else return false;
    }

    public override int GetHashCode() => text.GetHashCode();

    public static bool operator ==(Cliche? lhs, Cliche? rhs)
    {
        if (lhs is null) return rhs is null;
        if (rhs is null) return lhs is null;

        return lhs.Equals(rhs);
    }

    public static bool operator !=(Cliche? lhs, Cliche? rhs)
    {
        return !(lhs == rhs);
    }
}

public class ClicheCityStats
{
    public float catchiness;
    public float spread;

    public ClicheCityStats(float catchiness, float spread)
    {
        this.catchiness = catchiness;
        this.spread = spread;
    }
}