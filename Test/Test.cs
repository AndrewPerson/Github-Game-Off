using Godot;
using System.Linq;
using System.Threading.Tasks;
using NLP;
using Proxem.Word2Vec;

public partial class Test : Node2D
{
    public override void _Ready()
    {
        Catalyst.Models.English.Register();

        //TODO Change this to use built-in Godot file system. This will currently break when exported.
        var model = Word2Vec.LoadBinary("NLP/model.bin", normalize: true, encoding: System.Text.Encoding.UTF8);

        var bestWords = model.NClosestIndices("happy", 10).Select(i => model.Text[i]).ToArray();
        GD.Print(bestWords.Join(", "));

        Task.Run(async () =>
        {
            GD.Print(model.PhraseSimilarity(await Normaliser.NormalisePhrase("A watched pot never boils."), await Normaliser.NormalisePhrase("If you watch a kettle, it never boils.")));
        });
    }
}
