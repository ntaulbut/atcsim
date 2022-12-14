using Godot;

public partial class Aeroplane : Node, IAeroplane
{
    public const int SecondsInAnHour = 3600;

    [Export] public float Altitude { get; set; }
    [Export] public float TrueAirspeed { get; set; }
    [Export] public Vector2 PositionNm { get; set; }
    [Export] public Vector2 Velocity { get; set; }
    private float _heading;
    [Export] public float TrueHeading
    {
        get => _heading;
        set => _heading = Mathf.Wrap(value, 0, 360);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 airVector = Util.HeadingToVector(TrueHeading) * TrueAirspeed;
        Vector2 windVector = Util.HeadingToVector(Simulator.WindDirection) * Simulator.WindSpeed;
        Velocity = (airVector + windVector) / SecondsInAnHour * (float)delta;
        PositionNm += Velocity;
    }
}
