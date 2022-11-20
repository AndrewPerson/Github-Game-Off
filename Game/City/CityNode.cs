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
    private ClicheListNode clicheList = null!;

    private bool hovering;

    public override void _Ready()
    {
        nameLabel = GetNode<RichTextLabel>("%Name");
        indicator = GetNode("%Indicator");
        connections = GetNode("%Connections");
        clicheList = GetNode<ClicheListNode>("%ClicheList");

        Position = city.position;

        city.OnUpdateCliches += UpdateName;
        city.OnUpdateCliches += UpdateIndicator;
        city.OnUpdateConnections += UpdateConnections;

        clicheList.City = city;
        clicheList.Visible = false;

        UpdateName();
        UpdateIndicator();
        UpdateConnections();
    }

    private void UpdateName()
    {
        nameLabel.Clear();
        nameLabel.PushParagraph(HorizontalAlignment.Center, Control.TextDirection.Auto);
        nameLabel.PushColor(city.ControlledBy == GameNode.Instance.player ? new Color(0, 1, 0) : new Color(1, 0, 0));
        nameLabel.AddText(city.name);
        nameLabel.Pop();
        nameLabel.Pop();
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
                connectionNode.Connection = connection;
            }
            else
            {
                var connectionNode = connectionTemplate.Instantiate<ConnectionNode>();

                connectionNode.Connection = connection;

                connections.AddChild(connectionNode);
            }

            index++;
        }
    }

    public void OnMouseEnter()
    {
        hovering = true;
        clicheList.Visible = true;
    }

    public void OnMouseExit()
    {
        hovering = false;
        clicheList.Visible = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (hovering)
        {
            if (@event is InputEventMouseButton mouseButton)
            {
                if (mouseButton.ButtonIndex == MouseButton.Left)
                {
                    if (GameNode.Instance.connectionCreator.creatingConnection)
                    {
                        if (!mouseButton.Pressed)
                        {
                            GameNode.Instance.connectionCreator.ToggleConnectionCreation(this);
                        }
                    }
                    else
                    {
                        if (mouseButton.Pressed)
                        {
                            GameNode.Instance.connectionCreator.ToggleConnectionCreation(this);
                        }
                    }
                }
            }
        }
    }
}
