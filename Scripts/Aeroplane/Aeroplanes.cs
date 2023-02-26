using Godot;
using Guidance;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Aeroplanes : Node
{
    private const float MetresToNauticalMiles = 0.0005399568f;
    [Export] public AudioStreamPlayer AeroplaneEnterAudio;
    [Export] public AudioStreamPlayer AeroplaneExitAudio;

    private EntryPoint lastEntryPointUsed = null;

	private float timeSinceLastDeparture = 110;

    public void EnterAeroplane()
	{
		// Arrivals

		Random random = new();
		RadarConfig radarConfig = Simulator.RadarConfig;

		// Pick random entry point that wasn't used in the previous time period
		List<EntryPoint> availableEntryPoints = radarConfig.EntryPoints.ToList();
		availableEntryPoints.Remove(lastEntryPointUsed);
		if (availableEntryPoints.Count > 0)
		{
			EntryPoint entryPoint = availableEntryPoints[random.Next(0, availableEntryPoints.Count)];
			lastEntryPointUsed = entryPoint;
			// Add an aeroplane at the point
			Node node = GD.Load<PackedScene>("res://Scenes/Aeroplane.tscn").Instantiate();
			Aeroplane aeroplane = (Aeroplane)node;
			aeroplane.TrueAltitude = entryPoint.Level * 100;
			aeroplane.TrueHeading = entryPoint.Heading + radarConfig.MagneticVariation;
            aeroplane.PositionNm = Geo.RelativePositionNm(entryPoint.Waypoint.LatLon, radarConfig.LatLon);
            // Set up guidance
            if (entryPoint.Direct is not null)
			{
				aeroplane.TargetedWaypoint = Simulator.Waypoints[entryPoint.Direct.ResourceName];
				aeroplane.LateralGuidanceMode = new Direct(aeroplane);
            }
            else
            {
                aeroplane.SelectedHeading = (int)aeroplane.TrueHeading;
                aeroplane.LateralGuidanceMode = new HeadingHold();
            }
            aeroplane.SelectedAltitude = (int)aeroplane.TrueAltitude;
            aeroplane.VerticalGuidanceMode = new AltitudeHold(aeroplane);
            aeroplane.SelectedSpeed = (int)aeroplane.TrueAirspeed;
            AddChild(node);
            // Play sound
            AeroplaneEnterAudio.Play();
		}
		else
		{
			lastEntryPointUsed = null;
		}
	}

	public override void _Process(double delta)
	{
		// Departures

		if (timeSinceLastDeparture > 180)
		{
			Runway runway = Simulator.DepartureRunway;
            // Add an aeroplane on takeoff
            Node node = GD.Load<PackedScene>("res://Scenes/Aeroplane.tscn").Instantiate();
            Aeroplane aeroplane = (Aeroplane)node;
			aeroplane.IsDeparture = true;
			aeroplane.TrueAltitude = runway.Elevation + 1000;
			aeroplane.FlightPathAngle = 6;
			aeroplane.TrueHeading = runway.TrueBearing;
            // Position at runway end
            Vector2 threshold = Geo.RelativePositionNm(runway.ThresholdLatLon, Simulator.RadarConfig.LatLon);
            aeroplane.PositionNm = threshold + Util.HeadingToVector(runway.TrueBearing) * runway.Length * MetresToNauticalMiles;
			aeroplane.TrueAirspeed = 200;
			// Set up guidance
			aeroplane.SelectedSpeed = 250;
            aeroplane.SelectedHeading = (int)aeroplane.TrueHeading;
            aeroplane.LateralGuidanceMode = new HeadingHold();
			aeroplane.SelectedAltitude = 5000;
			aeroplane.VerticalGuidanceMode = new VerticalSpeed(aeroplane, 2500);
			aeroplane.ArmedVerticalGuidanceModes.Add(new AltitudeHold(aeroplane));
            AddChild(node);
            // Play sound
            AeroplaneEnterAudio.Play();
            // Reset timer
            timeSinceLastDeparture = 0;
        }

		timeSinceLastDeparture += (float)delta;
	}

    public void PlayAeroplaneExitAudio()
    {
        AeroplaneExitAudio.Play();
    }

    public override void _Ready()
	{
		EnterAeroplane();
	}
}
