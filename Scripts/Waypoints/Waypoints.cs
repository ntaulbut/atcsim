using Godot;
using System;
using System.Collections.Generic;

public partial class Waypoints : Node
{
    [Export] public Resource WaypointScene;
    // [Export] public Dictionary<string, Waypoint> waypoints = new Dictionary<string, Waypoint>();

    public override void _Ready()
    {
        PackedScene waypointScene = GD.Load<PackedScene>(WaypointScene.ResourcePath);
        // Add all the waypoints for the radar config to the scene
        foreach (WaypointData waypointData in Simulator.RadarConfig.Waypoints)
        {
            Node node = waypointScene.Instantiate();
            Waypoint waypoint = (Waypoint)node;
            waypoint.WaypointData = waypointData;
            // waypoints.Add(waypointData.ResourceName, waypoint);
            node.Name = waypointData.ResourceName;
            AddChild(node);
        }
    }
}
