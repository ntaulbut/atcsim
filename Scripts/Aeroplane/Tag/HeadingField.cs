using Godot;

public partial class HeadingField : LineEdit
{
    [Signal] public delegate void HeadingInstructionEventHandler(int heading, int turnDirection);

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
        else if (newText[0] == 'L' && newText.Substring(1).IsValidInt())
        {
            EmitSignal(nameof(HeadingInstruction), newText.Substring(1).ToInt(), (int)Guidance.TurnDirection.Left);
            Text = newText.Substring(1).PadLeft(3, '0');
            _oldText = Text;
        }
        // If the first character is R and the rest is a valid number, e.g. R180, R45
        else if (newText[0] == 'R' && newText.Substring(1).IsValidInt())
        {
            EmitSignal(nameof(HeadingInstruction), newText.Substring(1).ToInt(), (int)Guidance.TurnDirection.Right);
            Text = newText.Substring(1).PadLeft(3, '0');
            _oldText = Text;
        }
        else
        {
            // Cancel changes
            Text = _oldText;
        }

        ReleaseFocus();
    }

    public override void _Ready()
    {
        Text = Aeroplane.SelectedHeading.ToString().PadLeft(3, '0');
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
