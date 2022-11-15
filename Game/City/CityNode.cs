using Godot;

namespace Game;

public partial class CityNode : Node2D
{
	[Export]
	public PackedScene indicatorSliceTemplate = null!;

	[Export]
	public PackedScene connectionTemplate = null!;

	public City city = null!;
	private RichTextLabel nameLabel = null!;
	private Node indicator = null!;
	private Node connections = null!;
	
    public override void _Ready()
    {
		nameLabel = (RichTextLabel)FindChild("Name");
		indicator = FindChild("Indicator");
		connections = FindChild("Connections");

		Position = city.position;

		nameLabel.PushParagraph(HorizontalAlignment.Center, Control.TextDirection.Auto);
		nameLabel.AddText(city.name);
		nameLabel.Pop();

		city.OnUpdateCliches += UpdateIndicator;
		city.OnUpdateConnections += UpdateConnections;

		UpdateIndicator();
		UpdateConnections();
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

			slice.Colour = cliche.Colour;
			slice.Percentage = stats.spread;
			slice.PercentageOffset = cumulativePercentage;

			cumulativePercentage += stats.spread;

			index++;
		}
	}

	private void UpdateConnections()
	{
		for (int i = connections.GetChildCount() - 1; i >= city.connections.Count; i--)
		{
			connections.GetChild(i).QueueFree();
		}

		int index = 0;
		foreach (var connection in city.connections)
		{
			if (index < connections.GetChildCount())
			{
				var connectionNode = connections.GetChild<ConnectionNode>(index);
				connectionNode.connection = connection;
			}
			else
			{
				var connectionNode = connectionTemplate.Instantiate<ConnectionNode>();

				connectionNode.connection = connection;

				connections.AddChild(connectionNode);
			}

			index++;
		}
	}
}
