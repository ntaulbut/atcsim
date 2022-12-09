using Godot;
using System;

public partial class RadarSelector : VBoxContainer
{
	[Export]
	public Resource SimulatorScene;

    [Export]
	public OptionButton dropdown;

    [Export]
	public RadarConfig[] radarConfigs;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var radarConfig in radarConfigs)
        {
			dropdown.AddItem(radarConfig.ResourceName);
        }
	}

	public void GoButtonPressed()
    {
		Application.SelectedRadarConfig = radarConfigs[dropdown.Selected];
		GetTree().ChangeSceneToFile(SimulatorScene.ResourcePath);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
