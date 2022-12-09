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
}
