using Godot;

public partial class AltitudeField : LineEdit
{
	[Signal] public delegate void AltitudeInstructionEventHandler(int altitude);

	[Export] public Aeroplane Aeroplane;

	private string _oldText = "";

	public void OnChanged(string newText)
	{
		// Make all text entered uppercase, preserving the position of the caret
		int oldCaretColumn = CaretColumn;
		Text = newText.ToUpper();
		CaretColumn = oldCaretColumn;
	}

	public void OnSubmitted(string newText) 
	{
		// If the text entered is a valid integer, send an instruction using that value
		if (newText.IsValidInt())
		{
			EmitSignal(nameof(AltitudeInstruction), newText.ToInt() * 100);
			Text = newText.PadLeft(3, '0');
			_oldText = Text;
		}
		else
		{
			// Cancel changes
			Text = _oldText;
		}

		ReleaseFocus();
	}

	public void OnAeroplaneReady()
	{
		// Set the text to the initial selected altitude
		Text = (Aeroplane.SelectedAltitude / 100).ToString().PadLeft(3, '0');
		_oldText = Text;
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
