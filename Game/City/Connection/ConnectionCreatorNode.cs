using Godot;
using Game;

public partial class ConnectionCreatorNode : Node
{
    [Export]
    public PlaceholderConnectionNode placeholderConnection = null!;

    public bool creatingConnection;
    private CityNode? cityFrom;

    public void ToggleConnectionCreation(CityNode initiator)
    {
        if (creatingConnection)
        {
            creatingConnection = false;
            placeholderConnection.Visible = false;

            if (initiator == cityFrom)
            {
                cityFrom = null;
            }
            else
            {
                cityFrom?.city.ConnectTo(initiator.city);
                cityFrom = null;
            }
        }
        else
        {
            creatingConnection = true;
            placeholderConnection.Visible = true;

            cityFrom = initiator;

            placeholderConnection.From = initiator.city.position;
            placeholderConnection.To = initiator.city.position;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (creatingConnection)
        {
            if (@event is InputEventMouseMotion motion)
            {
                var viewport = GetViewport();
                var camera = viewport.GetCamera2d();
                var visibleRect = viewport.GetVisibleRect();
                placeholderConnection.To = camera.Position + (motion.GlobalPosition - visibleRect.Size / 2) / camera.Zoom;
            }

            if (@event is InputEventMouseButton button && !button.Pressed && button.ButtonIndex == MouseButton.Left)
            {
                ToggleConnectionCreation(cityFrom!);
            }
        }
    }
}