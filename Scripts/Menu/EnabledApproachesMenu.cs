using Godot;

public partial class EnabledApproachesMenu : VBoxContainer
{
	[Signal]
	public delegate void EnabledApproachesChangedEventHandler();

	public void EnableApproach(ILSApproach approach)
	{
		Simulator.EnabledILSApproaches.Add(approach);
		EmitSignal(nameof(EnabledApproachesChanged));
	}

	public void DisableApproach(ILSApproach approach)
	{
		Simulator.EnabledILSApproaches.Remove(approach);
		EmitSignal(nameof(EnabledApproachesChanged));
	}

	public void CheckboxToggled(bool pressed, ILSApproach approach)
	{
		if (pressed)
		{
			EnableApproach(approach);
		}
		else
		{
			DisableApproach(approach);
		}
	}

	public override void _Ready()
	{
		ILSApproach[] ILSApproaches = Simulator.RadarConfig.Airports[0].ILSApproaches;
		Simulator.EnabledILSApproaches.Clear();

		// Add a checkbox for each available approach and connect a signal to each
		foreach (ILSApproach approach in ILSApproaches)
		{
			CheckBox checkBox = new()
			{
				Text = approach.ResourceName
			};
			// Connect the signal with an extra value with the approach it controls
			checkBox.Toggled += (bool pressed) => CheckboxToggled(pressed, approach);
			AddChild(checkBox);
		}
	}
}
