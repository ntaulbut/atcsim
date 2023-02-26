using Godot;

public partial class EntryPoint : Resource
{
    [Export] public WaypointData Waypoint;
    [Export] public WaypointData Direct;
    [Export] public int Level;
    [Export] public int Heading;
}
