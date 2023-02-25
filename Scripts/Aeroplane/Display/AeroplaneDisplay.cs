using Godot;

public partial class AeroplaneDisplay : Node2D
{
    [Export] public Aeroplane Aeroplane;
    private Vector2 _displayPositionNm;

    private void Update()
    {
        // Sync position with aeroplane
        _displayPositionNm = Aeroplane.PositionNm;
    }

    public void DisplayUpdateTimeout()
    {
        // Update on the timeout of the display update timer
        Update();
    }

    public override void _Ready()
    {
        Update();
    }

    public override void _Process(double delta)
    {
        // Set position based on last known aeroplane position
        Position = Simulator.ScaledPosition(_displayPositionNm, GetViewportRect());
    }
}
