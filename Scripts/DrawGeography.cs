using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class DrawGeography : Node2D
{
	private RadarConfig _radarConfig;
	private List<List<Vector2>> _polyLines = new List<List<Vector2>>();

	public void GenerateTerrain(RadarConfig radarConfig)
	{
		_radarConfig = radarConfig;
		var polyLines = JSON.ParseString(radarConfig.GeoLines.data).AsGodotArray();
		foreach (var polyLine in polyLines)
		{
			List<Vector2> points = new();
			foreach (float[] point in polyLine.AsGodotArray())
			{
				Vector2 PointNm = Geo.RelativePositionNm(new Vector2(point[1], point[0]), radarConfig.LatLon);
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
			Vector2[] scaledPolyline = polyLine.Select(PointNm => _radarConfig.ScaledPosition(PointNm, GetViewportRect())).ToArray();
			DrawPolyline(scaledPolyline, _radarConfig.Style.CoastlineColour);
        }
    }
}
