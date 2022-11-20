using Godot;

namespace Game;

public partial class PlaceholderConnectionNode : ColorRect
{
    private Vector2 from = Vector2.Zero;
	public Vector2 From
    {
        get => from;
        set
        {
            from = value;
            UpdatePosition();
        }
    }

    private Vector2 to = Vector2.Zero;
    public Vector2 To
    {
        get => to;
        set
        {
            to = value;
            UpdatePosition();
        }
    }

	public override void _Ready()
	{
		UpdatePosition();
	}

    private void UpdatePosition()
    {
        float dist = from.DistanceTo(to);
        Size = new Vector2(dist, Size.y);
        Position = from + new Vector2(0, -PivotOffset.y);
        Rotation = (to - from).Angle();

        ((ShaderMaterial)Material).SetShaderParameter("size", new Vector2(dist / Size.y, 1));
    }
}
