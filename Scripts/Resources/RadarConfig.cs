using Godot;
using System;

public partial class RadarConfig : Resource
{
    public enum DisplayFixedBy {Width, Height, Compromise}
    [Export] public RadarStyle Style;
    [Export] public int WidthNm = 60;
    [Export] public int HeightNm = 60;
    [Export] public DisplayFixedBy FixedBy;
    [Export] public Vector2 LatLon;
    [Export] public JSONData GeoLines;
    [Export] public WaypointData[] Waypoints;

    public float Scale(Rect2 viewportRect)
    {
        return FixedBy switch
        {
            DisplayFixedBy.Width => viewportRect.Size.x / WidthNm,
            DisplayFixedBy.Height => viewportRect.Size.y / HeightNm,
            DisplayFixedBy.Compromise => (viewportRect.Size.x / WidthNm + viewportRect.Size.y / HeightNm) / 2,
            _ => throw new NotImplementedException()
        };
    }

    public Vector2 ScaledPosition(Vector2 position, Rect2 viewportRect)
    {
        return new Vector2(position.x, -position.y) * Scale(viewportRect);
    }
}
