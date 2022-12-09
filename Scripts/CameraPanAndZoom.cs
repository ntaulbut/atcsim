using Godot;
using System;

public partial class CameraPanAndZoom : Camera2D
{
	private Vector2 PositionNm;

	private Vector2 _initialCameraPositionNm;
	private Vector2 _initialMousePosition;

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("Zoom In"))
		{
			Simulator.Zoom *= (1f + Simulator.ZoomSpeed);
		}
		else if (inputEvent.IsActionPressed("Zoom Out"))
		{
			Simulator.Zoom *= (1f - Simulator.ZoomSpeed);
		}
		else if (inputEvent.IsActionPressed("Reset Camera"))
		{
			Simulator.Zoom = 1f;
		}
		Simulator.Zoom = Mathf.Clamp(Simulator.Zoom, 0.1f, 10f);
		Position = Simulator.ScaledPosition(PositionNm, GetViewportRect());
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Reset Camera"))
        {
			Position = Vector2.Zero;
			_initialCameraPositionNm = Vector2.Zero;
			_initialMousePosition = GetViewport().GetMousePosition();
		}

		// Set reference points for movement
		if (Input.IsActionJustPressed("Pan Camera"))
        {
			_initialMousePosition = GetViewport().GetMousePosition();
			_initialCameraPositionNm = PositionNm;
        }

		// Move the camera in the opposite direction to mouse movement, from the reference point
		if (Input.IsActionPressed("Pan Camera"))
        {
			Vector2 mouseDelta = GetViewport().GetMousePosition() - _initialMousePosition;
			Vector2 inverse = new Vector2(-mouseDelta.x, mouseDelta.y);
			PositionNm = _initialCameraPositionNm + inverse / Simulator.Scale(GetViewportRect());
		}

		Position = Simulator.ScaledPosition(PositionNm, GetViewportRect());
	}
}
