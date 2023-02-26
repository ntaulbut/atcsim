using Godot;

public partial class HeadingField : LineEdit
{
	[Signal] public delegate void HeadingInstructionEventHandler(int heading, int turnDirection);
	[Signal] public delegate void DirectInstructionEventHandler(string waypointName);

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
		// If just a number is entered e.g. 180, 45
		if (newText.IsValidInt())
		{
			EmitSignal(nameof(HeadingInstruction), newText.ToInt(), (int)Guidance.TurnDirection.Quickest);
			Text = newText.PadLeft(3, '0');
			_oldText = Text;
		}
		// If the first character is L and the rest is a valid number, e.g. L180, L45
		else if (newText[0] == 'L' && newText[1..].IsValidInt())
		{
			EmitSignal(nameof(HeadingInstruction), newText[1..].ToInt(), (int)Guidance.TurnDirection.Left);
			Text = newText[1..].PadLeft(3, '0');
			_oldText = Text;
		}
		// If the first character is R and the rest is a valid number, e.g. R180, R45
		else if (newText[0] == 'R' && newText[1..].IsValidInt())
		{
			EmitSignal(nameof(HeadingInstruction), newText[1..].ToInt(), (int)Guidance.TurnDirection.Right);
			Text = newText[1..].PadLeft(3, '0');
			_oldText = Text;
		}
		// If the entered text is the name of a waypoint
		else if (Simulator.Waypoints.ContainsKey(newText))
		{
			EmitSignal(nameof(DirectInstruction), newText);
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
		// If a waypoint is targeted
		if (Aeroplane.TargetedWaypoint is not null)
		{
			// Set the text to the name of the waypoint
			Text = Aeroplane.TargetedWaypoint.Name;
		}
		else
		{
			// Set the text to the initial selected heading
			Text = Aeroplane.SelectedHeading.ToString().PadLeft(3, '0');
		}
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
