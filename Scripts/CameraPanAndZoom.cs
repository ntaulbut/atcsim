using Godot;
using System.Reflection.Metadata;

public partial class CameraPanAndZoom : Camera2D
{
    private Vector2 PositionNm;

    private Vector2 _initialCameraPosition;
    private Vector2 _initialMousePosition;

    private const int MaxZoom = 20;
    private const int MinZoom = -20;

    private void SetReference()
    {
        _initialMousePosition = GetViewport().GetMousePosition();
        _initialCameraPosition = Position;
    }

    public override void _Input(InputEvent @inputEvent)
    {
        if (@inputEvent.IsActionPressed("Zoom In"))
        {
            if (Simulator.Zoom < MaxZoom)
            {
                Simulator.Zoom++;
                // Move the camera to compensate for stretching of distances
                Position *= 1f + Simulator.ZoomSpeed;
                SetReference();
            }
        }
        else if (@inputEvent.IsActionPressed("Zoom Out"))
        {
            if (Simulator.Zoom > MinZoom)
            {
                Simulator.Zoom--;
                // Move the camera to compensate for stretching of distances
                Position /= 1f + Simulator.ZoomSpeed;
                SetReference();
            }
        }
        else if (@inputEvent.IsActionPressed("Reset Camera"))
        {
            Simulator.Zoom = 0;
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Reset Camera"))
        {
            Position = Vector2.Zero;
            SetReference();
        }

        // Set reference points for movement
        if (Input.IsActionJustPressed("Pan Camera"))
        {
            SetReference();
        }

        // Move the camera in the opposite direction to mouse movement, from the reference point
        if (Input.IsActionPressed("Pan Camera"))
        {
            Vector2 mouseDelta = GetViewport().GetMousePosition() - _initialMousePosition;
            Position = _initialCameraPosition + new Vector2(-mouseDelta.x, -mouseDelta.y);
        }
    }
}
