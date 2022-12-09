using Godot;
using System;

public partial class AeroplaneDisplay : Node2D
{
    [Export] public Aeroplane Aeroplane;
    private Simulator _simulator;
    private Vector2 _displayPositionNm;

    public void DisplayUpdateTimeout()
    {
        _displayPositionNm = Aeroplane.PositionNm;
    }

    public override void _Ready()
    {
        _simulator = GetNode<Simulator>("/root/Simulator"); // get rid of this (?)
    }

    public override void _Process(double delta)
    {
        //DisplayPositionNm = Aeroplane.PositionNm;
        Position = _simulator.ScaledPosition(_displayPositionNm);
    }
}
