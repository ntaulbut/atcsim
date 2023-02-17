using Godot;

public partial class AeroplaneDisplay : Node2D
{
    [Export] public Aeroplane Aeroplane;
    private Vector2 _displayPositionNm;

    private void Update()
    {
        _displayPositionNm = Aeroplane.PositionNm;
    }

    public void DisplayUpdateTimeout()
    {
        Update();
    }

    public override void _Ready()
    {
        Update();
    }

    public override void _Process(double delta)
    {
        //DisplayPositionNm = Aeroplane.PositionNm;
        Position = Simulator.ScaledPosition(_displayPositionNm, GetViewportRect());
    }
}
