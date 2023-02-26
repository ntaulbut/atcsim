using Godot;
using System;

public partial class Timescale : HSlider
{
	public void OnValueChanged(float value)
	{
		Engine.TimeScale = value;
		GD.Print("Changed timescale to ", value);
	}
}
