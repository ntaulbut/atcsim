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
		float distanceBetweenMarkers = 1.67f;

		foreach (ILSApproach approach in Simulator.EnabledILSApproaches)
		{
			Rect2 viewport = GetViewportRect();
			// Main line
			Vector2 threshold = RelativePositionNm(approach.RunwayThresholdLatLon, Simulator.RadarConfig.LatLon);
			Vector2 direction = HeadingToVector(OppositeHeading(approach.LocaliserHeading));
			Vector2 end = threshold + direction * distanceMarkers * distanceBetweenMarkers;
			DrawLine(Simulator.ScaledPosition(threshold, viewport), Simulator.ScaledPosition(end, viewport), Colors.White);
			// Distance markers
			Vector2 normal = HeadingToVector(Mathf.Wrap(approach.LocaliserHeading + 90, 0, 360));
			for (int i = 0; i < distanceMarkers + 1; i++)
			{
				Vector2 point = threshold + direction * i * distanceBetweenMarkers;
				DrawLine(OppositeNormalPoint(point, normal, -0.2f), OppositeNormalPoint(point, normal, 0.2f), Colors.White);
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

	public void OnEnabledApproachesChanged()
	{
		QueueRedraw();
	}
}
