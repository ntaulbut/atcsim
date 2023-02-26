using Godot;

public partial class SpeedField : LineEdit
{
	[Signal] public delegate void SpeedInstructionEventHandler(int speed);

	[Export] private Aeroplane _aeroplane;
	[Export] private Tag _tag;
	private string _oldText = "";

	public void OnSubmitted(string newText)
	{
		// If just a number is entered e.g. 180,250
		if (newText.IsValidInt())
		{
			int speed = newText.ToInt();
			// If value is within a valid range
			if (speed > 0 && speed < 350)
			{
				EmitSignal(nameof(SpeedInstruction), speed);
				Text = newText.PadLeft(3, '0');
				_oldText = Text;
			}
		}
		else
		{
			// Cancel changes
			Text = _oldText;
		}

		ReleaseFocus();
	}

	public override void _Process(double delta)
	{
		// Hide this button if the tag isn't hovered
		if (_tag.Hovering)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	public void OnAeroplaneReady()
	{
		// Set the text to the initial selected speed
		Text = _aeroplane.SelectedSpeed.ToString().PadLeft(3, '0');
		_oldText = Text;
		Hide();
	}

	public override void _Input(InputEvent @event)
	{
		// If the left mouse button is pressed outside of the field, de-focus it and cancel changes
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.ButtonIndex == MouseButton.Left && eventMouseButton.Pressed == true)
			{
				if (!GetRect().HasPoint(((InputEventMouseButton)MakeInputLocal(eventMouseButton)).Position))
				{
					ReleaseFocus();
					// Cancel changes
					Text = _oldText;
				}
			}
		}
	}
}
