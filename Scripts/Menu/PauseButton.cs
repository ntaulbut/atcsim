using Godot;

public partial class PauseButton : Button
{
	public void OnToggled(bool pressed)
	{
		if (pressed)
		{
			Engine.TimeScale = 0;
			GD.Print("Paused");
		}
		else
		{
			Engine.TimeScale = 1;
			GD.Print("Unpaused");
		}

		ReleaseFocus();
	}
}
