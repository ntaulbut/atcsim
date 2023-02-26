using Godot;

public partial class RadarSelector : VBoxContainer
{
	[Export]
	public Resource SimulatorScene;

	[Export]
	public OptionButton dropdown;

	[Export]
	public RadarConfig[] radarConfigs;

	public override void _Ready()
	{
		// Add a dropdown item for each radar config
		foreach (var radarConfig in radarConfigs)
		{
			dropdown.AddItem(radarConfig.ResourceName);
		}
	}

	public void GoButtonPressed()
	{
		// Start the simulation with the specified radar config
		Simulator.RadarConfig = radarConfigs[dropdown.Selected];
		GetTree().ChangeSceneToFile(SimulatorScene.ResourcePath);
	}
}
