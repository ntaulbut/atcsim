using Godot;
using System;
using System.Collections.Generic;

public partial class Simulator : Node
{
    public static RadarConfig RadarConfig;
    public static List<ILSApproach> EnabledILSApproaches = new();
    public static Runway DepartureRunway;
    public static Dictionary<string, Waypoint> Waypoints = new();

    public static float WindDirection;
    public static float WindSpeed;

    public static float ZoomSpeed = 0.1f; // Percentage change in zoom level
    public static int Zoom = 0; // Zoom increment

    private static Rect2 _viewportRect;
    private static float _previousScale;

    [Signal]
    public delegate void ScaleChangedEventHandler();

    public static float Scale(Rect2 viewportRect)
    {
        // Calculate the scale based on the initial view size and the zoom level
        _viewportRect = viewportRect;
        float zoomValue = Mathf.Pow(1 + ZoomSpeed, Zoom);
        return RadarConfig.FixedBy switch
        {
            RadarConfig.DisplayFixedBy.Width => (viewportRect.Size.X / RadarConfig.WidthNm) * zoomValue,
            RadarConfig.DisplayFixedBy.Height => (viewportRect.Size.Y / RadarConfig.HeightNm) * zoomValue,
            RadarConfig.DisplayFixedBy.Compromise => (viewportRect.Size.X / RadarConfig.WidthNm + viewportRect.Size.Y / RadarConfig.HeightNm) / 2 * zoomValue,
            _ => throw new NotImplementedException()
        };
    }

    public static Vector2 ScaledPosition(Vector2 position, Rect2 viewportRect)
    {
        return new Vector2(position.X, -position.Y) * Scale(viewportRect);
    }

    public override void _Process(double delta)
    {
        if (RadarConfig is not null)
        {
            // Emit signal if scale has changed (for many reasons)
            float scale = Scale(_viewportRect);
            if (scale != _previousScale)
            {
                EmitSignal(nameof(ScaleChanged));
            }
            _previousScale = scale;
        }
    }
}
