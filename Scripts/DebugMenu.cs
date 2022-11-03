using Godot;
using System;

public partial class DebugMenu : PanelContainer
{
	private Simulator _simulator;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_simulator = GetTree().Root.GetChild<Simulator>(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_simulator.WindSpeed = (int)((Slider)FindChild("WindSpeed")).Value;
		((Label)FindChild("WindSpeedLabel")).Text = _simulator.WindSpeed.ToString();

		GetNode<Aeroplane>("/root/Simulator/Aeroplanes/Aeroplane").TrueHeading = (int)((Slider)FindChild("Heading")).Value;
		((Label)FindChild("HeadingLabel")).Text = GetNode<Aeroplane>("/root/Simulator/Aeroplanes/Aeroplane").TrueHeading.ToString();

		_simulator.WindDirection = (int)((Slider)FindChild("WindDirection")).Value;
		((Label)FindChild("WindDirectionLabel")).Text = _simulator.WindDirection.ToString();
	}
}
