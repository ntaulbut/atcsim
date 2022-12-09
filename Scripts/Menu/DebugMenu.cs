using Godot;
using System;

public partial class DebugMenu : PanelContainer
{
	public override void _Process(double delta)
	{
		Simulator.WindSpeed = (int)((Slider)FindChild("WindSpeed")).Value;
		((Label)FindChild("WindSpeedLabel")).Text = Simulator.WindSpeed.ToString();

		GetNode<Aeroplane>("/root/Root/Aeroplanes/Aeroplane").TrueHeading = (int)((Slider)FindChild("Heading")).Value;
		((Label)FindChild("HeadingLabel")).Text = GetNode<Aeroplane>("/root/Root/Aeroplanes/Aeroplane").TrueHeading.ToString();

		Simulator.WindDirection = (int)((Slider)FindChild("WindDirection")).Value;
		((Label)FindChild("WindDirectionLabel")).Text = Simulator.WindDirection.ToString();
	}
}
