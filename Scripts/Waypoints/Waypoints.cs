using Godot;
using System;
using System.Collections.Generic;

public partial class Waypoints : Node
{
	[Export] public Resource WaypointScene;
	// [Export] public Dictionary<string, Waypoint> waypoints = new Dictionary<string, Waypoint>();

	public void InstantiateWaypoints(RadarConfig radarConfig)
	{
		PackedScene waypointScene = GD.Load<PackedScene>(WaypointScene.ResourcePath);
		foreach (WaypointData waypointData in radarConfig.Waypoints)
		{
			Node node = waypointScene.Instantiate();
			Waypoint waypoint = (Waypoint)node;
			waypoint.WaypointData = waypointData;
			waypoint.RadarConfig = radarConfig;
			// waypoints.Add(waypointData.ResourceName, waypoint);
			node.Name = waypointData.ResourceName;
			AddChild(node);
		}
	}
}
