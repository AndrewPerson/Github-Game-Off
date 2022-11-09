using Godot;

namespace Game;

public partial class CityNode : Node2D
{
	[Export]
	public PackedScene indicatorSliceTemplate = null!;

	public City city = null!;
	private RichTextLabel nameLabel = null!;
	private Control indicator = null!;
	
    public override void _Ready()
    {
		//DEBUG code. Remove when done
		city.clicheStats[new Cliche("Cliche A")] = new ClicheCityStats(0.5f, 0.4f);
		city.clicheStats[new Cliche("Cliche B")] = new ClicheCityStats(0.3f, 0.3f);
		city.clicheStats[new Cliche("Cliche C")] = new ClicheCityStats(0.2f, 0.3f);

		nameLabel = (RichTextLabel)FindChild("Name");
		indicator = (Control)FindChild("Indicator");

		Position = city.position;

		nameLabel.PushParagraph(HorizontalAlignment.Center, Control.TextDirection.Auto);
		nameLabel.AddText(city.name);
		nameLabel.Pop();

		city.OnUpdateCliches += UpdateIndicator;

		UpdateIndicator();
    }

	private Color GetColor(Cliche cliche)
	{
		var hash = cliche.GetHashCode();

		var r = (hash & 0xFF) / 255.0f;
		var g = ((hash >> 8) & 0xFF) / 255.0f;
		var b = ((hash >> 16) & 0xFF) / 255.0f;

		return new Color(r, g, b);
	}

	private void UpdateIndicator()
	{
		for (int i = indicator.GetChildCount() - 1; i >= city.clicheStats.Count; i--)
		{
			indicator.GetChild(i).QueueFree();
		}

		int index = 0;
		float cumulativePercentage = 0;
		foreach (var (cliche, stats) in city.clicheStats)
		{
			IndicatorSliceNode slice;
			if (index < indicator.GetChildCount())
			{
				slice = (IndicatorSliceNode)indicator.GetChild(index);
			}
			else
			{
				slice = (IndicatorSliceNode)indicatorSliceTemplate.Instantiate();
				
				//Although this is already set in the scene, it needs to be done in code as well for the anchors to readjust
				slice.AnchorsPreset = (int)Control.LayoutPreset.FullRect;

				indicator.AddChild(slice);
			}

			slice.Colour = GetColor(cliche);
			slice.Percentage = stats.spreadPercentage;
			slice.PercentageOffset = cumulativePercentage;

			cumulativePercentage += stats.spreadPercentage;

			index++;
		}
	}
}
