using Proxem.NumNet;
using Proxem.NumNet.Single;
using Proxem.Word2Vec;
using Catalyst;
using Mosaik.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NLP;

static class Word2VecUtils
{
    public static float PhraseSimilarity(IList<Array<float>> p1, IList<Array<float>> p2)
    {
        var v1 = p1.Aggregate((a, b) => a + b) / p1.Count;
        var v2 = p2.Aggregate((a, b) => a + b) / p2.Count;

        return (v1.VectorDot(v2) / MathF.Sqrt(v1.Sum(x => x * x)) * MathF.Sqrt(v2.Sum(x => x * x)) + 1) / 2;
    }

    public static int[] NClosestIndices(this Word2Vec self, string word, int n)
    {
        float[] bestDistances = new float[n];
        int[] bestIndices = new int[n];
        self.NBest(self[word], bestDistances, bestIndices);

        return bestIndices;
    }
}

static class Normaliser
{
    static readonly Task<Pipeline> pipeline = Pipeline.ForAsync(Language.English);

    public static async Task<string[]> NormalisePhrase(string phrase)
    {
        IDocument doc = new Document(phrase, Language.English);
        doc = (await pipeline).ProcessSingle(doc);

        return doc.ToTokenList().Where(t => t.POS == PartOfSpeech.ADJ ||
                                            t.POS == PartOfSpeech.ADV ||
                                            t.POS == PartOfSpeech.NOUN ||
                                            t.POS == PartOfSpeech.VERB)
                                .Select(t => t.Lemma)
                                .Select(w => new string(w.ToLower()
                                                         .Where(c => char.IsLetterOrDigit(c))
                                                         .ToArray()))
                                .ToArray();
    }
}