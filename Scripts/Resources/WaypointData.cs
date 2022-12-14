using Godot;

public partial class WaypointData : Resource
{
    public enum Type {RNAV, VORDME, VOR, NDB}
    [Export] public Type Basis;
    [Export] public Vector2 LatLon;
}
