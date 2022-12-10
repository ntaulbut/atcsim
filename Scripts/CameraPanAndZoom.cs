using Godot;
using System;

public partial class CameraPanAndZoom : Camera2D
{
    private Vector2 PositionNm;

    private Vector2 _initialCameraPosition;
    private Vector2 _initialMousePosition;
    private float _initialZoom;

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("Zoom In"))
        {
            Simulator.Zoom *= (1f + Simulator.ZoomSpeed);
            //Position *= (1f + Simulator.ZoomSpeed);
            //_initialCameraPosition *= (1f + Simulator.ZoomSpeed);
        }
        else if (inputEvent.IsActionPressed("Zoom Out"))
        {
            Simulator.Zoom *= (1f - Simulator.ZoomSpeed);
            //Position *= (1f - Simulator.ZoomSpeed);
            //_initialCameraPosition *= (1f - Simulator.ZoomSpeed);
        }
        else if (inputEvent.IsActionPressed("Reset Camera"))
        {
            Simulator.Zoom = 1f;
        }
        Simulator.Zoom = Mathf.Clamp(Simulator.Zoom, 0.1f, 10f);
        
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Reset Camera"))
        {
            Position = Vector2.Zero;
            _initialMousePosition = GetViewport().GetMousePosition();
        }

        // Set reference points for movement
        if (Input.IsActionJustPressed("Pan Camera"))
        {
            _initialMousePosition = GetViewport().GetMousePosition();
            _initialCameraPosition = Position;
            _initialZoom = Simulator.Zoom;
        }

        // Move the camera in the opposite direction to mouse movement, from the reference point
        if (Input.IsActionPressed("Pan Camera"))
        {
            Vector2 mouseDelta = GetViewport().GetMousePosition() - _initialMousePosition;
            Vector2 inverseMouseDelta = new Vector2(-mouseDelta.x, -mouseDelta.y);
            //Position = (_initialCameraPosition + inverseMouseDelta) * (Simulator.Zoom / _initialZoom);
            GD.Print(Simulator.Zoom / _initialZoom);
        }
    }
}
