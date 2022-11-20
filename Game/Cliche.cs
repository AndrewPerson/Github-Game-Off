using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Proxem.NumNet;
using NLP;

namespace Game;

public class Cliche
{
    public readonly string text;
    public readonly Player player;

    private readonly Task<Array<float>[]> Vectors = null!;

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

    public Cliche(string text, Player player)
    {
        this.text = text;
        this.player = player;

        Vectors = GetVectors();
    }

    public float GetCatchiness(City city)
    {
        var catchiness = 0.0f;
        foreach (var vector in city.likeVectors)
        {
            var similarity = Word2VecUtils.PhraseSimilarity(Vectors.Result, new[] { vector });
            catchiness += similarity - 0.5f;
        }

        return catchiness;
    }

    public async Task<Array<float>[]> GetVectors()
    {
        var pieces = (await Normaliser.NormalisePhrase(text)).Where(word => GameNode.Instance.model.Text.Contains(word));

        return pieces.Select(word => GameNode.Instance.model[word]).ToArray();
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj == null) return false;

        if (obj is Cliche cliche2)
        {
            return cliche2.text == text && cliche2.player == player;
        }
        else return false;
    }

    public override int GetHashCode() => HashCode.Combine(text, player);

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