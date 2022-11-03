using Godot;
using System;

public partial class Simulator : Node2D
{
    [Export]
    public RadarConfig RadarConfig;

    public float WindDirection;
    public float WindSpeed;

	[Signal]
	public delegate void StartEventHandler(RadarConfig radarConfig);

    public Vector2 ScaledPosition(Vector2 position)
    {
        return new Vector2(position.x, -position.y) * RadarConfig.Scale(GetViewportRect());
    }

    public override void _Ready()
	{
		EmitSignal("Start", RadarConfig);
	}

    public override void _Process(double delta)
	{
	}
}
