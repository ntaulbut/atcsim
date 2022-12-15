using Godot;
using System;

public partial class DebugBarText : Label
{
	public override void _Process(double delta)
	{
		Text = string.Format("{0}fps {1}ms",
			Engine.GetFramesPerSecond(),
			Mathf.Round((float)delta * 1000)
		);
	}
}
