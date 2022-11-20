using System.Diagnostics.CodeAnalysis;

namespace Game;

public class Player
{
    public string name;

    public Player(string name)
    {
        this.name = name;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is null) return false;
        
        if (obj is Player player2)
        {
            return player2.name == name;
        }
        else return false;
    }

    public override int GetHashCode() => name.GetHashCode();

    public static bool operator ==(Player? lhs, Player? rhs)
    {
        if (lhs is null) return rhs is null;
        if (rhs is null) return lhs is null;

        return lhs.Equals(rhs);
    }

    public static bool operator !=(Player? lhs, Player? rhs)
    {
        return !(lhs == rhs);
    }
}