using Godot;
using System;

public partial class AeroplaneDisplay : Node2D
{
    public Aeroplane Aeroplane;
    private Simulator _simulator;
    private Vector2 _displayPositionNm;

    public void DisplayUpdateTimeout()
    {
        _displayPositionNm = Aeroplane.PositionNm;
    }

    public override void _Ready()
    {
        _simulator = GetTree().Root.GetChild<Simulator>(0);
        Aeroplane = GetParent<Aeroplane>();
    }

    public override void _Process(double delta)
    {
        //DisplayPositionNm = Aeroplane.PositionNm;
        Position = _simulator.ScaledPosition(_displayPositionNm);
    }
}
