using Godot;
using System;

public partial class HeadingField : LineEdit
{
    [Signal] public delegate void HeadingInstructionEventHandler(int heading, int turnDirection);

    private String _text = "";

    public void OnChanged(string newText)
    {
        int oldCaretColumn = CaretColumn;
        Text = newText.ToUpper();
        CaretColumn = oldCaretColumn;
    }

    public void OnSubmitted(string newText) 
    {
        if (newText.IsValidInt())
        {
            EmitSignal(nameof(HeadingInstruction), newText.ToInt(), (int)Guidance.TurnDirection.Quickest);
            Text = newText.PadLeft(3, '0');
        }
        else if (newText[0] == 'L' && newText.Substring(1).IsValidInt())
        {
            EmitSignal(nameof(HeadingInstruction), newText.Substring(1).ToInt(), (int)Guidance.TurnDirection.Left);
            Text = newText.Substring(1).PadLeft(3, '0');
        }
        else if (newText[0] == 'R' && newText.Substring(1).IsValidInt())
        {
            EmitSignal(nameof(HeadingInstruction), newText.Substring(1).ToInt(), (int)Guidance.TurnDirection.Right);
            Text = newText.Substring(1).PadLeft(3, '0');
        }

        ReleaseFocus();
    }

    public override void _Input(InputEvent @event)
    {
        // If the left mouse button is pressed outside of the field, de-focus it
        if (@event is InputEventMouseButton eventMouseButton)
            if (eventMouseButton.ButtonIndex == MouseButton.Left && eventMouseButton.Pressed == true)
                if (!GetRect().HasPoint(((InputEventMouseButton)MakeInputLocal(eventMouseButton)).Position))
                    ReleaseFocus();
    }
}
