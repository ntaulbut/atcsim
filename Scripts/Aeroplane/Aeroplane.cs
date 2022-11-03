using Godot;
using System;

public partial class Aeroplane : Node
{
    public const int SecondsInAnHour = 3600;

    [Export] public float Altitude;
    [Export] public float TrueAirspeed;
    private float _heading;
    [Export] public float TrueHeading
    {
        get => _heading;
        set => _heading = Mathf.Wrap(value, 0, 360);
    }

    [Export] public Vector2 PositionNm;
    [Export] public Vector2 Velocity;
    private Simulator _simulator;

    private Vector2 HeadingToVector(float heading)
    {
        // Take a heading between 0-360 degrees and convert it to a normalized vector in that direction
        Vector2 quadrant = new Vector2(1, 1);
        float theta = heading;
        if (heading > 270)
        {
            theta = 360 - heading;
            quadrant = new Vector2(-1, 1);
        }
        else if (heading > 180)
        {
            theta = heading - 180;
            quadrant = new Vector2(-1, -1);
        }
        else if (heading > 90)
        {
            theta = 180 - heading;
            quadrant = new Vector2(1, -1);
        }
        return new Vector2(Mathf.Sin(Mathf.DegToRad(theta)), Mathf.Sin(Mathf.DegToRad(90 - theta))).Normalized() * quadrant;
    }

    public override void _Ready()
    {
        _simulator = GetTree().Root.GetChild<Simulator>(0);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 airVector = HeadingToVector(TrueHeading) * TrueAirspeed;
        Vector2 windVector = HeadingToVector(_simulator.WindDirection) * _simulator.WindSpeed;
        Velocity = (airVector + windVector) / SecondsInAnHour * (float)delta;
        PositionNm += Velocity;
    }
}
