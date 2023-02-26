using Godot;

public partial class Waypoints : Node
{
    [Export] public Resource WaypointScene;

    public override void _Ready()
    {
        PackedScene waypointScene = GD.Load<PackedScene>(WaypointScene.ResourcePath);
        Simulator.Waypoints.Clear();
        // Add all the waypoints for the radar config to the scene
        foreach (WaypointData waypointData in Simulator.RadarConfig.Waypoints)
        {
            Node node = waypointScene.Instantiate();
            Waypoint waypoint = (Waypoint)node;
            waypoint.WaypointData = waypointData;
            node.Name = waypointData.ResourceName;
            AddChild(node);
            // Register waypoint
            Simulator.Waypoints.Add(waypointData.ResourceName, waypoint);
        }
    }
}
