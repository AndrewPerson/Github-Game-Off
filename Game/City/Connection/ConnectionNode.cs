using Godot;
using System;

namespace Game;

public partial class ConnectionNode : ColorRect
{
	public Connection connection = null!;

	public override void _Ready()
	{
		float dist = connection.Length;
		Size = new Vector2(dist, Size.y);
		Position = connection.from.position + new Vector2(0, -PivotOffset.y);
		Rotation = connection.Angle;

		((ShaderMaterial)Material).SetShaderParameter("size", new Vector2(dist / Size.y, 1));
	}
}
