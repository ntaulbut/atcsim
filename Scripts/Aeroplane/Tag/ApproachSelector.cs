using Godot;

public partial class ApproachSelector : OptionButton
{
    [Signal] public delegate void ApproachInstructionEventHandler(int approach_index);
    [Signal] public delegate void CancelApproachInstructionEventHandler();

    [Export] public Aeroplane Aeroplane;
    [Export] private Tag _tag;

    public void OnItemSelected(int index)
    {
        ReleaseFocus();

        // If selecting an approach, send approach instruction, otherwise send cancel approach instruction
        if (index != 0)
        {
            EmitSignal(nameof(ApproachInstruction), index - 1);
            SetItemText(0, "Cancel");
        }
        else
        {
            EmitSignal(nameof(CancelApproachInstruction));
            SetItemText(0, "Unset");
        }
    }

    public override void _Ready()
    {
        Hide();

        // Populate the dropdown with the available approaches
        foreach (ILSApproach approach in Simulator.RadarConfig.Airports[0].ILSApproaches)
        {
            AddItem(approach.ResourceName);
        }
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

    public override void _Input(InputEvent @event)
    {
        // If the left mouse button is pressed outside of the dropdown, de-focus it
        if (@event is InputEventMouseButton eventMouseButton)
        {
            if (eventMouseButton.ButtonIndex == MouseButton.Left && eventMouseButton.Pressed == true)
            {
                if (!GetRect().HasPoint(((InputEventMouseButton)MakeInputLocal(eventMouseButton)).Position))
                {
                    ReleaseFocus();
                }
            }
        }
    }
}
