using Godot;

public partial class RadarConfig : Resource
{
    [Export] public Airport[] Airports;
    [Export] public EntryPoint[] EntryPoints;
    [Export] public string[] Callsigns;
    public enum DisplayFixedBy {Width, Height, Compromise}
    [Export] public RadarStyle Style;
    [Export] public int WidthNm = 60;
    [Export] public int HeightNm = 60;
    [Export] public DisplayFixedBy FixedBy;
    [Export] public Vector2 LatLon;
    [Export] public JSONData GeoLines;
    [Export] public WaypointData[] Waypoints;
    [Export] public WaypointData[] ExitPoints;
}
