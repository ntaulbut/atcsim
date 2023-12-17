using Godot;
using Guidance;
using System;
using System.Collections.Generic;

public partial class Aeroplane : Node, IAeroplane
{
	[Export] public string Callsign { get; set; }
	[Export] public float TrueAltitude { get; set; }
	[Export] public float TrueAirspeed { get; set; }
	[Export] public Vector2 PositionNm { get; set; }
	[Export] public Vector2 GroundVector { get; set; }
	private float _heading;
	[Export] public float TrueHeading
	{
		get => _heading;
		set => _heading = Mathf.Wrap(value, 0, 360);
	}
	public float VerticalSpeed {
		get => TrueAirspeed * Mathf.Sin(Mathf.DegToRad(FlightPathAngle)) * KnotsToFeetPerSecond; // feet/s
	}
	// For departures
	public bool IsDeparture;
	public Waypoint AssignedExitPoint;

	// Flight
	public float Roll = 0; // Deg/s
	public float FlightPathAngle = 0; // Deg

	// Guidance
	public ILateralMode LateralGuidanceMode;
	public IVerticalMode VerticalGuidanceMode;
	public List<IArmableLateralMode> ArmedLateralGuidanceModes = new();
	public List<IArmableVerticalMode> ArmedVerticalGuidanceModes = new();
	// FCU
	public int SelectedAltitude;
	public int SelectedHeading;
	public int SelectedSpeed;
	public ILSApproach Approach;
	public Waypoint TargetedWaypoint;

	// Constants
	public const int SecondsInAnHour = 3600;
	public const float StandardRateTurn = 3f; // Deg/s/s
	public const float RollRate = 0.5f; // Deg/s/s
	public const float PitchRate = 0.17f; // Deg/s
	public const float SpeedChangeRate = 1f; // knots/s
	// Unit conversions
	public const float FeetPerMinuteToKnots = 0.00987473f;
	public const float KnotsToFeetPerSecond = 1.68781f;
	public const float NauticalMilesToFeet = 6076.115f;

	private void OnHeadingInstruction(int heading, int turnDirection)
	{
		SelectedHeading = heading;
		LateralGuidanceMode = new HeadingSelect(this, (TurnDirection)turnDirection);
	}

	private void OnDirectInstruction(string waypointName)
	{
		TargetedWaypoint = Simulator.Waypoints[waypointName];
		LateralGuidanceMode = new Direct(this);
	}

	private void OnAltitudeInstruction(int altitude)
	{
		SelectedAltitude = altitude;
		VerticalGuidanceMode = new VerticalSpeed(this, 1900);
		if (ArmedVerticalGuidanceModes.Find(mode => mode is AltitudeHold) is null)
			ArmedVerticalGuidanceModes.Add(new AltitudeHold(this));
	}

	private void OnApproachInstruction(int approach_index)
	{
		Approach = Simulator.EnabledILSApproaches[approach_index];
		ArmedLateralGuidanceModes.Add(new Localiser(this));
		ArmedVerticalGuidanceModes.Add(new Glideslope(this));
		GD.Print("Cleared ", Approach.ResourceName, " approach!");
	}

	private void OnCancelApproachInstruction()
	{
		GD.Print("Approach cancelled");
		Approach = null;
		// If currently on the approach
		if (VerticalGuidanceMode is Glideslope || LateralGuidanceMode is Localiser)
		{
			// Climb to selected altitude
			VerticalGuidanceMode = new VerticalSpeed(this, 2500);
			ArmedVerticalGuidanceModes.Add(new AltitudeHold(this));
			// Maintain current heading
			LateralGuidanceMode = new HeadingHold();
		}
		else
		{
			// Disarm approach modes
			ArmedLateralGuidanceModes.RemoveAll(mode => mode is Localiser);
			ArmedVerticalGuidanceModes.RemoveAll(mode => mode is Glideslope);
		}
	}

	private void OnSpeedInstruction(int speed)
	{
		SelectedSpeed = speed;
	}

	public override void _Ready()
	{
		Random random = new();
		Callsign = Simulator.RadarConfig.Callsigns[random.Next(0, Simulator.RadarConfig.Callsigns.Length - 1)] + random.Next(1, 999).ToString().PadLeft(3, '0');

		PhysicsUpdate(0.01f);
	}

	public void Exit()
	{
		QueueFree();
		GetParent<Aeroplanes>().PlayAeroplaneExitAudio();
	}

	private void PhysicsUpdate(float delta)
	{
		// Lateral guidance
		// Activate armed lateral guidance modes if condition is met
		for (int i = 0; i < ArmedLateralGuidanceModes.Count; i++)
		{
			IArmableLateralMode lateralMode = ArmedLateralGuidanceModes[i];
			if (lateralMode?.Activate() ?? false)
			{
				LateralGuidanceMode = lateralMode;
				ArmedLateralGuidanceModes.Remove(lateralMode);
			}
		}
		// Change lateral guidance mode if the current mode requests
		LateralGuidanceMode = LateralGuidanceMode.NewMode() ?? LateralGuidanceMode;
		// Move roll towards the commanded value
		Roll = Mathf.MoveToward(Roll, LateralGuidanceMode.RollCommand(delta), RollRate * delta);

		TrueHeading += Roll * delta;

		// Vertical guidance
		// Activate armed vertical guidance modes if condition is met
		for (int i = 0; i < ArmedVerticalGuidanceModes.Count; i++)
		{
			IArmableVerticalMode verticalMode = ArmedVerticalGuidanceModes[i];
			if (verticalMode?.Activate() ?? false)
			{
				VerticalGuidanceMode = verticalMode;
				ArmedVerticalGuidanceModes.Remove(verticalMode);
			}
		}
		// Change vertical guidance mode if the current mode requests
		VerticalGuidanceMode = VerticalGuidanceMode.NewMode() ?? VerticalGuidanceMode;
		// Move flight path angle towards commanded value
		FlightPathAngle = Mathf.MoveToward(FlightPathAngle, VerticalGuidanceMode.FlightPathAngleCommand(), PitchRate * delta);

		// Move speed towards selected speed
		TrueAirspeed = Mathf.MoveToward(TrueAirspeed, SelectedSpeed, SpeedChangeRate * delta);
		// Update altitude
		TrueAltitude += VerticalSpeed * delta;
		// Update position
		Vector2 airVector = Util.HeadingToVector(TrueHeading) * TrueAirspeed * Mathf.Cos(Mathf.Abs(Mathf.DegToRad(FlightPathAngle)));
		Vector2 windVector = Util.HeadingToVector(Simulator.WindDirection) * Simulator.WindSpeed;
		GroundVector = airVector + windVector;
		PositionNm += GroundVector / SecondsInAnHour * delta;

		// Remove the aeroplane if it lands
		if (VerticalGuidanceMode is Glideslope && TrueAltitude < Approach.GlideslopeElevation + 50)
		{
			Exit();
		}
		else if (TrueAltitude < 0)
		{
			Exit();
		}
		// Remove the aeroplane if it reaches its assigned exit point
		if (IsDeparture && LateralGuidanceMode is Direct)
		{
			if (TargetedWaypoint == AssignedExitPoint)
			{
				if (PositionNm.DistanceSquaredTo(TargetedWaypoint.PositionNm) < 0.01f)
				{
					Exit();
				}
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		PhysicsUpdate((float)delta);
	}
}
