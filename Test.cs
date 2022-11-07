using Godot;
using System.Threading.Tasks;
using NLP;
using Proxem.Word2Vec;

public partial class Test : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Catalyst.Models.English.Register();

		var model = Word2Vec.LoadBinary("model.bin", normalize: true, encoding: System.Text.Encoding.UTF8);

		Task.Run(async () =>
		{
			GD.Print(model.PhraseSimilarity(await Normaliser.NormalisePhrase("A watched pot never boils."), await Normaliser.NormalisePhrase("If you watch a kettle, it never boils.")));
		});
	}
}