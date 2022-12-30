using Godot;

public partial class Aeroplane : Node, IAeroplane
{
    [Export] public float Altitude { get; set; }
    [Export] public float TrueAirspeed { get; set; }
    [Export] public Vector2 PositionNm { get; set; }
    [Export] public Vector2 GroundVector { get; set; }
    private float _heading;
    [Export] public float TrueHeading
    {
        get => _heading;
        set => _heading = Mathf.Wrap(value, 0, 360);
    }
    public float Roll = 0; // Deg/s

    // Guidance
    private Guidance.LateralMode _lateralGuidanceMode;
    // private Guidance.VerticalMode _verticalGuidanceMode;

    public const int SecondsInAnHour = 3600;

    public const float StandardRateTurn = 3f; // Deg/s/s
    public const float RollRate = 0.5f; // Deg/s/s

    private void OnHeadingInstruction(float heading, int turnDirection)
    {
        _lateralGuidanceMode = new Guidance.HeadingSelect(this, heading, (Guidance.TurnDirection)turnDirection);
    }

    public override void _PhysicsProcess(double delta)
    {
        Roll = Mathf.MoveToward(Roll, _lateralGuidanceMode.RollCommand(), RollRate * (float)delta);
        TrueHeading += Roll * (float)delta;

        Vector2 airVector = Util.HeadingToVector(TrueHeading) * TrueAirspeed;
        Vector2 windVector = Util.HeadingToVector(Simulator.WindDirection) * Simulator.WindSpeed;
        GroundVector = airVector + windVector;
        GD.Print(GroundVector.Length());
        PositionNm += GroundVector / SecondsInAnHour * (float)delta;
    }

    public override void _Ready()
    {
        _lateralGuidanceMode = new Guidance.HeadingSelect(this, TrueHeading, Guidance.TurnDirection.Quickest);
    }
}