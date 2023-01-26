using Godot;
using System;

public partial class Callsign2 : Label
{
	[Export] private Tag _tag;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_tag.Hovering)
		{
            Show();
        }
		else
		{
            Hide();
        }
	}
}
