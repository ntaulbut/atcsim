using Godot;
using System;

public partial class HeadingField : LineEdit
{
    [Signal] public delegate void HeadingInputEventHandler(int heading, int turnDirection);

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
            EmitSignal(nameof(HeadingInput), newText.ToInt(), (int)TurnDirection.Quickest);
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
