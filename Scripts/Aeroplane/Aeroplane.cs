using Godot;

public partial class Aeroplane : Node, IAeroplane
{
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

    // Flight
    public float Roll = 0; // Deg/s
    public float FlightPathAngle = 0; // Deg

    // Guidance
    public Guidance.ILateralMode LateralGuidanceMode;
    public Guidance.IVerticalMode VerticalGuidanceMode;

    public const int SecondsInAnHour = 3600;
    public const float FeetPerMinuteToKnots = 0.00987473f;
    public const float KnotsToFeetPerSecond = 1.68781f;

    public const float StandardRateTurn = 3f; // Deg/s/s
    public const float RollRate = 0.5f; // Deg/s/s
    public const float PitchRate = 0.17f; // Deg/s

    private void OnHeadingInstruction(float heading, int turnDirection)
    {
        LateralGuidanceMode = new Guidance.HeadingSelect(this, heading, (Guidance.TurnDirection)turnDirection);
    }

    private void OnAltitudeInstruction(float altitude)
    {
        VerticalGuidanceMode = new Guidance.VerticalSpeed(this, altitude, 1900);
    }

    public override void _EnterTree()
    {
        LateralGuidanceMode = new Guidance.HeadingSelect(this, TrueHeading, Guidance.TurnDirection.Quickest);
        VerticalGuidanceMode = new Guidance.AltitudeHold();
    }

    public override void _PhysicsProcess(double delta)
    {
        // Lateral guidance
        LateralGuidanceMode = LateralGuidanceMode.NewMode() ?? LateralGuidanceMode;
        Roll = Mathf.MoveToward(Roll, LateralGuidanceMode.RollCommand(), RollRate * (float)delta);
        TrueHeading += Roll * (float)delta;

        // Vertical guidance
        VerticalGuidanceMode = VerticalGuidanceMode.NewMode() ?? VerticalGuidanceMode;
        FlightPathAngle = Mathf.MoveToward(FlightPathAngle, VerticalGuidanceMode.FlightPathAngleCommand(), PitchRate * (float)delta);
        TrueAltitude += VerticalSpeed * (float)delta;

        Vector2 airVector = Util.HeadingToVector(TrueHeading) * TrueAirspeed * Mathf.Cos(Mathf.Abs(Mathf.DegToRad(FlightPathAngle)));
        Vector2 windVector = Util.HeadingToVector(Simulator.WindDirection) * Simulator.WindSpeed;
        GroundVector = airVector + windVector;
        PositionNm += GroundVector / SecondsInAnHour * (float)delta;
    }
}