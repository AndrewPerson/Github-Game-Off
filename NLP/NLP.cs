using Proxem.NumNet.Single;
using Proxem.Word2Vec;
using Catalyst;
using Mosaik.Core;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace NLP;

static class Word2VecImproved
{
    public static float PhraseSimilarity(this Word2Vec self, string[] p1, string[] p2)
    {
        var v1 = p1.Select(w => self[w]).Aggregate((a, b) => a + b) / p1.Length;
        var v2 = p2.Select(w => self[w]).Aggregate((a, b) => a + b) / p2.Length;

        return (v1.VectorDot(v2) / MathF.Sqrt(v1.Sum(x => x * x)) * MathF.Sqrt(v2.Sum(x => x * x)) + 1) / 2;
    }
}

static class Normaliser
{
    static readonly ILemmatizer lemmatiser = LemmatizerStore.Get(Language.English);

    public static async Task<string[]> NormalisePhrase(string phrase)
    {
        var nlp = await Pipeline.ForAsync(Language.English);
        IDocument doc = new Document(phrase, Language.English);
        doc = nlp.ProcessSingle(doc);

        return doc.ToTokenList().Where(t => t.POS == PartOfSpeech.ADJ ||
                                            t.POS == PartOfSpeech.ADV ||
                                            t.POS == PartOfSpeech.NOUN ||
                                            t.POS == PartOfSpeech.VERB)
                                .Select(t => lemmatiser.GetLemma(t))
                                .Select(w => new string(w.ToLower()
                                                         .Where(c => char.IsLetterOrDigit(c))
                                                         .ToArray()))
                                .ToArray();
    }
}