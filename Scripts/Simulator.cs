using Godot;
using System;

public partial class Simulator : Node2D
{
    public static RadarConfig RadarConfig;

    public static float WindDirection;
    public static float WindSpeed;

    public static float ZoomSpeed = 0.1f; // Percentage change in zoom level
    public static int Zoom = 0; // Zoom increment

    private static Rect2 _viewportRect;
    private static float _previousScale;

    [Signal] public delegate void ScaleChangedEventHandler();

    public static new float Scale(Rect2 viewportRect)
    {
        _viewportRect = viewportRect;
        float zoomValue = 1f * Mathf.Pow(1 + ZoomSpeed, Zoom);
        return RadarConfig.FixedBy switch
        {
            RadarConfig.DisplayFixedBy.Width => (viewportRect.Size.x / RadarConfig.WidthNm) * zoomValue,
            RadarConfig.DisplayFixedBy.Height => (viewportRect.Size.y / RadarConfig.HeightNm) * zoomValue,
            RadarConfig.DisplayFixedBy.Compromise => (viewportRect.Size.x / RadarConfig.WidthNm + viewportRect.Size.y / RadarConfig.HeightNm) / 2 * zoomValue,
            _ => throw new NotImplementedException()
        };
    }

    public static Vector2 ScaledPosition(Vector2 position, Rect2 viewportRect)
    {
        return new Vector2(position.x, -position.y) * Scale(viewportRect);
    }

    public override void _Process(double delta)
    {
        if (RadarConfig is not null)
        {
            float scale = Scale(_viewportRect);
            if (scale != _previousScale)
            {
                EmitSignal(nameof(ScaleChanged));
            }
            _previousScale = scale;
        }
    }

    public override void _Ready()
    {
        
    }
}
