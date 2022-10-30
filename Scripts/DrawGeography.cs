using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class DrawGeography : Node2D
{
	private List<List<Vector2>> _polyLines = new List<List<Vector2>>();

	public override void _Ready()
	{
		var polyLines = JSON.ParseString(Session.RadarConfig.GeoLines.data).AsGodotArray();
		foreach (var polyLine in polyLines)
		{
			List<Vector2> points = new();
			foreach (float[] point in polyLine.AsGodotArray())
			{
				Vector2 PointNm = Geo.RelativePositionNm(new Vector2(point[1], point[0]), Session.RadarConfig.LatLon);
				points.Add(PointNm);
			}
			_polyLines.Add(points);
		}
	}

    public override void _Draw()
    {
		GD.Print("Drawing terrain");
		foreach(List<Vector2> polyLine in _polyLines)
        {
			Vector2[] scaledPolyline = polyLine.Select(polyLine => Session.ScaledPosition(polyLine, GetViewportRect())).ToArray();
			DrawPolyline(scaledPolyline, Session.RadarConfig.Style.CoastlineColour);
        }
    }
}
