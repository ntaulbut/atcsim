using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class HistoryDots : Node2D
{
	[Export] public Aeroplane Aeroplane;

	private List<Vector2> _previousPositions = new();
	private Vector2 _previousPosition;

	private Simulator _simulator;

	public override void _Ready()
	{
		_previousPositions = Enumerable.Repeat(Vector2.Zero, 10).ToList();
		_simulator = GetNode<Simulator>("/root/Simulator");
		GenerateDots(10);
	}

	private void GenerateDots(int dots)
    {
		RadarStyle style = _simulator.RadarConfig.Style;

		for (int i = 0; i < dots; i++)
        {
			Sprite2D dot = new Sprite2D();
			dot.Texture = style.HistoryDotTexture;
			dot.Modulate = style.HistoryDotsColourGradient.Sample(i / (float)dots);
			AddChild(dot);
		}
    }

	private void MoveDots()
    {
		for (int i = 0; i < 10; i++)
		{
			GetChild<Sprite2D>(i).Position = _simulator.RadarConfig.ScaledPosition(_previousPositions[i], GetViewportRect());
		}
	}

	public void DisplayUpdateTimeout()
    {
		MoveDots();

		_previousPosition = Aeroplane.PositionNm;
		_previousPositions.Insert(0, _previousPosition);
    }

    public override void _Draw()
    {
		MoveDots();
    }
}