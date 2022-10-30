using Godot;
using System;

public partial class RadarConfig : Resource
{
    public enum Display {Width, Height}
    [Export] public RadarStyle Style;
    [Export] public int WidthNm = 60;
    [Export] public int HeightNm = 60;
    [Export] public Display FixedBy;
    [Export] public Vector2 LatLon;
    [Export] public JSONData GeoLines;
    [Export] public WaypointData[] Waypoints;
}
