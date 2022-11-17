using Godot;
using Game;

public partial class ClicheIndicatorNode : Control
{
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

	private void SetClicheText(string text)
	{
		GetNode<RichTextLabel>("%ClicheText").Text = text;
	}

	private void SetClicheColour(Color colour)
	{
		GetNode<ColorRect>("%ClicheDot").Color = colour;
	}

	private void SetClicheSpread(float spread)
	{
		GetNode<RichTextLabel>("%ClicheSpread").Text = $"{Mathf.RoundToInt(spread * 100).ToString().PadLeft(2, '0')}%";
	}
}
