using Godot;
using System;

public partial class Simulator : Node2D
{
    public static RadarConfig RadarConfig;

    public static float WindDirection;
    public static float WindSpeed;

    public Vector2 ScaledPosition(Vector2 position) // phase this out
    {
        return new Vector2(position.x, -position.y) * RadarConfig.Scale(GetViewportRect());
    }

    public override void _Process(double delta)
	{
	}
}
