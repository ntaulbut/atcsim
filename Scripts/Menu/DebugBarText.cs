using Godot;

public partial class DebugBarText : Label
{
	double worst_1 = 0;
	double worst_1_age = 0;

	public override void _Process(double delta)
	{
		double fps = Engine.GetFramesPerSecond();
		double frame_time = delta * 1000;

		worst_1_age++;
		if (frame_time > worst_1 || worst_1_age > 1000)
		{
			worst_1 = frame_time;
			worst_1_age = 0;
		}

		Text = string.Format("{1}fps {2}ms, 0.1% low: {0}ms", Mathf.Round(worst_1), fps, Mathf.Round(frame_time));
	}
}
