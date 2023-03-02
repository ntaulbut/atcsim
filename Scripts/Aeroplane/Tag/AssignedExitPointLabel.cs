using Godot;

public partial class AssignedExitPointLabel : Label
{
	[Export] public Aeroplane Aeroplane;
    [Export] private Tag _tag;

    public void OnAeroplaneReady()
	{
		if (Aeroplane.IsDeparture)
		{
			Text = Aeroplane.AssignedExitPoint.Name;
		}
		else
		{
			Text = "";
		}
	}

    public override void _Ready()
    {
        Hide();
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
}
