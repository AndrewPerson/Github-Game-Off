using System.Diagnostics.CodeAnalysis;

namespace Game;

public class Cliche
{
    public readonly string text;

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

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(Cliche? lhs, Cliche? rhs)
    {
        if (lhs == null) return rhs == null;
        if (rhs == null) return lhs == null;

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
    public float spreadPercentage;

    public ClicheCityStats(float catchiness, float spreadPercentage)
    {
        this.catchiness = catchiness;
        this.spreadPercentage = spreadPercentage;
    }
}