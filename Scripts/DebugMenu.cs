using Godot;
using System;

public partial class DebugMenu : PanelContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Session.WindSpeed = (int)((Slider)FindChild("WindSpeed")).Value;
		((Label)FindChild("WindSpeedLabel")).Text = Session.WindSpeed.ToString();

		GetNode<Aeroplane>("/root/Root/Aeroplanes/Aeroplane").TrueHeading = (int)((Slider)FindChild("Heading")).Value;
		((Label)FindChild("HeadingLabel")).Text = GetNode<Aeroplane>("/root/Root/Aeroplanes/Aeroplane").TrueHeading.ToString();

		Session.WindDirection = (int)((Slider)FindChild("WindDirection")).Value;
		((Label)FindChild("WindDirectionLabel")).Text = Session.WindDirection.ToString();
	}
}
