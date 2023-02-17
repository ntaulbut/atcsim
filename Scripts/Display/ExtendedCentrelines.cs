using Godot;
using static Geo;
using static Util;


public partial class ExtendedCentrelines : Node2D
{
    private Vector2 OppositeNormalPoint(Vector2 start, Vector2 normal, float distance)
    {
        return Simulator.ScaledPosition(start + normal * distance, GetViewportRect());
    }

    public override void _Draw()
    {
        int distanceMarkers = 6;
        float distanceBetweenMarkers = 2;

        foreach (ILSApproach approach in Simulator.RadarConfig.Airports[0].ILSApproaches)
        {
            Rect2 viewport = GetViewportRect();
            // main line
            Vector2 threshold = RelativePositionNm(approach.RunwayThresholdLatLon, Simulator.RadarConfig.LatLon);
            Vector2 direction = HeadingToVector(OppositeHeading(approach.LocaliserHeading));
            Vector2 end = threshold + direction * (distanceMarkers - 1) * distanceBetweenMarkers;
            DrawLine(Simulator.ScaledPosition(threshold, viewport), Simulator.ScaledPosition(end, viewport), Colors.White);
            // distance markers
            Vector2 normal = HeadingToVector(Mathf.Wrap(approach.LocaliserHeading + 90, 0, 360));
            for (int i = 0; i < distanceMarkers; i++)
            {
                Vector2 point = threshold + direction * i * distanceBetweenMarkers;
                DrawLine(OppositeNormalPoint(point, normal, -0.25f), OppositeNormalPoint(point, normal, 0.25f), Colors.White);
            }
        }
    }

    public override void _EnterTree()
    {
        GetNode<Simulator>("/root/Simulator").ScaleChanged += OnScaleChanged;
    }

    public override void _ExitTree()
    {
        GetNode<Simulator>("/root/Simulator").ScaleChanged -= OnScaleChanged;
    }

    public void OnScaleChanged()
    {
        QueueRedraw();
    }
}
