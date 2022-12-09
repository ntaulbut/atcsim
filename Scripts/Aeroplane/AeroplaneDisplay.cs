using Godot;
using System;

public partial class AeroplaneDisplay : Node2D
{
    [Export] public Aeroplane Aeroplane;
    private Vector2 _displayPositionNm;

    public void DisplayUpdateTimeout()
    {
        _displayPositionNm = Aeroplane.PositionNm;
    }

    public override void _Process(double delta)
    {
        //DisplayPositionNm = Aeroplane.PositionNm;
        Position = Simulator.ScaledPosition(_displayPositionNm, GetViewportRect());
    }
}
