using Godot;

public partial class DeparturesSelection : OptionButton
{
	public override void _Ready()
	{
		Simulator.DepartureRunway = Simulator.RadarConfig.Airports[0].Runways[0];

		// Add runway options to dropdown
		foreach (Runway runway in Simulator.RadarConfig.Airports[0].Runways)
		{
			AddItem(runway.ResourceName);
		}
	}

	public void OnItemSelected(int index)
	{
		Simulator.DepartureRunway = Simulator.RadarConfig.Airports[0].Runways[index];

		ReleaseFocus();
	}
}
