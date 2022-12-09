using Godot;
using System;

public partial class CameraPanning : Camera2D
{
	private Vector2 _initialCameraPosition;
	private Vector2 _initialMousePosition;

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Reset Camera"))
        {
			Position = Vector2.Zero;
			_initialCameraPosition = Vector2.Zero;
			_initialMousePosition = GetViewport().GetMousePosition();
		}

		// Set reference points for movement
		if (Input.IsActionJustPressed("Pan Camera"))
        {
			_initialMousePosition = GetViewport().GetMousePosition();
			_initialCameraPosition = Position;
        }

		// Move the camera in the opposite direction to mouse movement, from the reference point
		if (Input.IsActionPressed("Pan Camera"))
        {
			Vector2 mouseDelta = GetViewport().GetMousePosition() - _initialMousePosition;
			Vector2 inverse = new Vector2(-mouseDelta.x, -mouseDelta.y);
			Position = _initialCameraPosition + inverse;
		}
	}
}
