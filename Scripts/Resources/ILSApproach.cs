using Godot;

public partial class ILSApproach : Resource
{
    [Export] public int GlideslopeElevation;
    [Export] public Vector2 GlideslopeLatLon;
    [Export] public float GlideslopeAngle = 3.0f;
    [Export] public Vector2 LocaliserLatLon;
    [Export] public float LocaliserHeading;
    [Export] public Vector2 RunwayThresholdLatLon;
}
