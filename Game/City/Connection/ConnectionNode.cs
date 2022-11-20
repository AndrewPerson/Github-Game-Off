using Godot;

namespace Game;

public partial class ConnectionNode : ColorRect
{
    private Connection connection = null!;
    public Connection Connection
    {
        get => connection;
        set
        {
            connection = value;
            UpdatePosition();
        }
    }

    public override void _Ready()
    {
        Material = (Material)Material.Duplicate();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        float dist = connection.Length;
        Size = new Vector2(dist, Size.y);
        Position = connection.from.position + new Vector2(0, -PivotOffset.y);
        Rotation = connection.Angle;

        ((ShaderMaterial)Material).SetShaderParameter("size", new Vector2(dist / Size.y, 1));
    }
}
