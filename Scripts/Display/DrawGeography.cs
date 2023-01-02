using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class DrawGeography : Node2D
{
    private readonly List<List<Vector2>> _polyLines = new();

    public override void _Ready()
    {
        var polyLines = JSON.ParseString(Simulator.RadarConfig.GeoLines.data).AsGodotArray();
        foreach (var polyLine in polyLines)
        {
            List<Vector2> points = new();
            foreach (float[] point in polyLine.AsGodotArray())
            {
                Vector2 PointNm = Geo.RelativePositionNm(new Vector2(point[1], point[0]), Simulator.RadarConfig.LatLon);
                points.Add(PointNm);
            }
            _polyLines.Add(points);
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

    public override void _Draw()
    {
        GD.Print("Drawing terrain");
        foreach(List<Vector2> polyLine in _polyLines)
        {
            Vector2[] scaledPolyline = polyLine.Select(PointNm => Simulator.ScaledPosition(PointNm, GetViewportRect())).ToArray();
            DrawPolyline(scaledPolyline, Simulator.RadarConfig.Style.CoastlineColour);
        }
    }

    public void OnScaleChanged()
    {
        QueueRedraw();
    }
}
