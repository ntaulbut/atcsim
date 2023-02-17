using Godot;

public partial class ExtendedCentrelines : Node2D
{
    public override void _Draw()
    {
        foreach (ILSApproach approach in Simulator.RadarConfig.Airports[0].ILSApproaches)
        {
            Vector2 threshold = Geo.RelativePositionNm(approach.RunwayThresholdLatLon, Simulator.RadarConfig.LatLon);
            Vector2 direction = Util.HeadingToVector(Util.OppositeHeading(approach.LocaliserHeading));
            Vector2 end = threshold + direction * 10f;
            DrawLine(Simulator.ScaledPosition(threshold, GetViewportRect()), Simulator.ScaledPosition(end, GetViewportRect()), Colors.White);
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
