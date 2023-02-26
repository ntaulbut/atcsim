using Godot;

public interface IAeroplane
{
    public string Callsign { get; set; }
    public float TrueAltitude { get; set; }
    public float TrueAirspeed { get; set; }
    public float TrueHeading { get; set; }
    public Vector2 PositionNm { get; set; }
    public Vector2 GroundVector { get; set; }
}
