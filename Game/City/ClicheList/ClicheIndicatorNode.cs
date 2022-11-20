using Godot;
using Game;

public partial class ClicheIndicatorNode : Control
{
	private ColorRect clicheDot = null!;
	private RichTextLabel clicheSpread = null!;
	private RichTextLabel clicheText = null!;

	private Cliche cliche = null!;
	public Cliche Cliche
	{
		get => cliche;
		set
		{
			cliche = value;
			SetClicheText(cliche.text);
			SetClicheColour(cliche.Colour);
		}
	}

	private ClicheCityStats clicheStats = null!;
	public ClicheCityStats ClicheStats
	{
		get => clicheStats;
		set
		{
			clicheStats = value;
			SetClicheSpread(clicheStats.spread);
		}
	}

	public override void _Ready()
	{
		clicheDot = GetNode<ColorRect>("%ClicheDot");
		clicheSpread = GetNode<RichTextLabel>("%ClicheSpread");
		clicheText = GetNode<RichTextLabel>("%ClicheText");
	}

	private void SetClicheText(string text)
	{
		clicheText.Clear();
		clicheText.PushColor(GameNode.Instance.player == cliche.player ? new Color(0, 1, 0) : new Color(1, 0, 0));
		clicheText.AddText(text);
		clicheText.Pop();
	}

	private void SetClicheColour(Color colour)
	{
		clicheDot.Color = colour;
	}

	private void SetClicheSpread(float spread)
	{
		clicheSpread.Clear();
		clicheSpread.PushColor(GameNode.Instance.player == cliche.player ? new Color(0, 1, 0) : new Color(1, 0, 0));
		clicheSpread.AddText($"{Mathf.RoundToInt(spread * 100).ToString().PadLeft(2, '0')}%");
		clicheSpread.Pop();
	}
}
