using Godot;
using System;

public partial class AeroplaneDisplay : Node2D
{
    public Aeroplane Aeroplane;
    private Vector2 _displayPositionNm;

    public override void _Ready()
    {
        Aeroplane = GetParent<Aeroplane>();
    }

    public override void _Process(double delta)
    {
        //DisplayPositionNm = Aeroplane.PositionNm;
        Position = Session.ScaledPosition(_displayPositionNm, GetViewportRect());
    }

    public void DisplayUpdateTimeout()
    {
        _displayPositionNm = Aeroplane.PositionNm;
    }
}
