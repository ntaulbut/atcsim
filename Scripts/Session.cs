using Godot;
using Godot.Collections;
using System;

public partial class Session : Node
{
    public static RadarConfig RadarConfig;

    public static double DateTime;

    public static int WindSpeed;
    public static int WindDirection;
    
    public override void _Ready()
    {
        RadarConfig = GD.Load<RadarConfig>("res://Resources/Radar Configs/Hiroshima.tres");
        DateTime = Time.GetUnixTimeFromSystem();

        //GD.Print(Mathf.Lerp(-7, 1425, 97 / 100f)* 3.28084);
    }

    public override void _Process(double delta)
    {
        DateTime += delta;
    }

    public static float Scale(Rect2 viewportRect)
    {
        return RadarConfig.FixedBy switch
        {
            RadarConfig.Display.Width => viewportRect.Size.x / RadarConfig.WidthNm,
            RadarConfig.Display.Height => viewportRect.Size.y / RadarConfig.HeightNm,
            _ => throw new NotImplementedException()
        };
    }

    public static Vector2 ScaledPosition(Vector2 position, Rect2 viewportRect)
    {
        return new Vector2(position.x, -position.y) * Scale(viewportRect);
    }
}
