using System.Diagnostics.CodeAnalysis;

namespace Game;

struct Cliche
{
    string text;

    public Cliche(string text)
    {
        this.text = text;
    }

    public float GetCatchiness(City city)
    {
        throw new System.NotImplementedException("Catchiness not implemented");
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        if (obj.GetType() == typeof(Cliche))
        {
            Cliche other = (Cliche)obj;
            return other.text == text;
        }
        else return false;
    }

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(Cliche lhs, Cliche rhs)
    {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Cliche lhs, Cliche rhs)
    {
        return !lhs.Equals(rhs);
    }
}