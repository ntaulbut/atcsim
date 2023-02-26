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

        if (index != 0)
        {
            // If selecting an approach (the unset/cancel option will be index 0),
            // send the approach instruction with selected index
            EmitSignal(nameof(ApproachInstruction), index - 1);
            SetItemText(0, "Cancel");
        }
        else
        {
            // If the cancel option was selected, send the cancel approach instruction
            EmitSignal(nameof(CancelApproachInstruction));
            SetItemText(0, "Unset");
        }
    }

    public void Populate()
    {
        // Remove all existing dropdown items apart from the first
        Clear();
        AddItem("Unset");
        // Add dropdown items for enabled approaches
        foreach (ILSApproach approach in Simulator.EnabledILSApproaches)
        {
            AddItem(approach.ResourceName);
        }
    }

    public void OnEnabledApproachesChanged()
    {
        Populate();
    }

    public override void _Ready()
    {
        Hide();
        Populate();
        GetNode<EnabledApproachesMenu>("/root/Root/CanvasLayer/Enabled Approaches Menu Container/Enabled Approaches Menu").EnabledApproachesChanged += OnEnabledApproachesChanged;
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
