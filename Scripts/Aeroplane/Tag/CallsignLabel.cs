using Godot;

public partial class CallsignLabel : Label
{
	[Export] public Aeroplane Aeroplane;

	public void OnAeroplaneReady()
	{
		Text = Aeroplane.Callsign;
	}
}
