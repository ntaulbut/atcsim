using Godot;

public partial class Runway : Resource
{
    [Export] public Vector2 ThresholdLatLon;
    [Export] public float TrueBearing;
    [Export] public float Length;
    [Export] public float Elevation;
}
