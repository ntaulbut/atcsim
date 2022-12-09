using Godot;
using System;

public partial class Simulator : Node2D
{
    public static RadarConfig RadarConfig;

    public static float WindDirection;
    public static float WindSpeed;

    private static float ZoomSpeed = 0.1f;
    private static float _zoom = 1f;

    private static Rect2 _viewportRect;
    private static float _previousScale;

    [Signal] public delegate void ScaleChangedEventHandler();

    public static new float Scale(Rect2 viewportRect)
    {
        _viewportRect = viewportRect;
        return RadarConfig.FixedBy switch
        {
            RadarConfig.DisplayFixedBy.Width => (viewportRect.Size.x / RadarConfig.WidthNm) * _zoom,
            RadarConfig.DisplayFixedBy.Height => (viewportRect.Size.y / RadarConfig.HeightNm) * _zoom,
            RadarConfig.DisplayFixedBy.Compromise => (viewportRect.Size.x / RadarConfig.WidthNm + viewportRect.Size.y / RadarConfig.HeightNm) / 2 * _zoom,
            _ => throw new NotImplementedException()
        };
    }

    public static Vector2 ScaledPosition(Vector2 position, Rect2 viewportRect)
    {
        return new Vector2(position.x, -position.y) * Scale(viewportRect);
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("Zoom In"))
        {
            _zoom *= (1f + ZoomSpeed);
        }
        else if (inputEvent.IsActionPressed("Zoom Out"))
        {
            _zoom *= (1f - ZoomSpeed);
        }
        else if (inputEvent.IsActionPressed("Reset Camera"))
        {
            _zoom = 1f;
        }
        _zoom = Mathf.Clamp(_zoom, 0.1f, 10f);
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
