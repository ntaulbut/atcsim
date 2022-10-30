using Godot;
using System;

public partial class HeadingIndicator : Node2D
{
	private AeroplaneDisplay _aeroplaneDisplay;

	[Export] public Color Color;
	[Export] public int Start = 10;
	[Export] public int Length = 40;

	public override void _Ready()
	{
		_aeroplaneDisplay = GetParent<AeroplaneDisplay>();
	}

	public override void _Draw()
	{
		Vector2 screenVelocity = new Vector2(_aeroplaneDisplay.Aeroplane.Velocity.x, -_aeroplaneDisplay.Aeroplane.Velocity.y);
		Vector2 direction = screenVelocity.Normalized();
		Vector2 start = Position + direction * Start;
		Vector2 end = start + direction * Length;
		DrawLine(start, end, Color);
	}

    public override void _Process(double delta)
    {
        QueueRedraw();
    }
}
