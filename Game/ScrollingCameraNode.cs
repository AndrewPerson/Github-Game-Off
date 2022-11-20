using Godot;
using System;

public partial class ScrollingCameraNode : Camera2D
{
    [Export]
    public float speed = 500;

    [Export]
    public float mouseSpeed = 1;

    [Export]
    public float zoomSpeed = 1;

    [Export]
    public float minZoom = 1;

    [Export]
    public float maxZoom = 5;

    private float FloatZoom
    {
        get => 1 / Zoom.x;
        set => Zoom = new Vector2(1 / value, 1 / value);
    }

    public override void _Ready()
    {
        FloatZoom = minZoom;
    }

    public override void _Process(double delta)
    {
        float forwards = Input.GetActionStrength("backwards") - Input.GetActionStrength("forwards");
        float left = Input.GetActionStrength("right") - Input.GetActionStrength("left");

        Position += new Vector2(left, forwards) * (float)delta * speed * FloatZoom;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsMouseButtonPressed(MouseButton.Right))
        {
            if (@event is InputEventMouseMotion mouseMotion)
            {
                Position -= mouseMotion.Relative * mouseSpeed * FloatZoom;
            }
        }
        else if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.IsPressed())
            {
                float zoomMovement = (mouseButton.ButtonIndex == MouseButton.WheelUp ? -1 :
                             mouseButton.ButtonIndex == MouseButton.WheelDown ? 1 :
                             0) * zoomSpeed;


                FloatZoom = Mathf.Clamp(FloatZoom + zoomMovement, minZoom, maxZoom);
            }
        }
    }
}
