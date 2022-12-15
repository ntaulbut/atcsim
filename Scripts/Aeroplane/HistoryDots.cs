using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class HistoryDots : Node2D
{
    [Export] public Aeroplane Aeroplane;

    private List<Vector2> _previousPositions = new();
    private Vector2 _previousPosition;

    public override void _Ready()
    {
        //_previousPositions = Enumerable.Repeat(Vector2.Zero, 10).ToList();
        GetNode<Simulator>("/root/Simulator").ScaleChanged += OnScaleChanged;
    }

    private void AddDot()
    {
        RadarStyle style = Simulator.RadarConfig.Style;
        Sprite2D dot = new Sprite2D();
        dot.Texture = style.HistoryDotTexture;
        dot.Modulate = style.HistoryDotsColourGradient.Sample(GetChildCount() / (float)style.HistoryDotsCount);
        AddChild(dot);
    }

    private void UpdateDotsPositions()
    {
        for (int i = 0; i < GetChildCount(); i++)
        {
            GetChild<Sprite2D>(i).Position = Simulator.ScaledPosition(_previousPositions[i], GetViewportRect());
        }
    }

    public void DisplayUpdateTimeout()
    {
        _previousPosition = Aeroplane.PositionNm;
        _previousPositions.Insert(0, _previousPosition);

        if (GetChildCount() < Simulator.RadarConfig.Style.HistoryDotsCount)
        {
            AddDot();
        }

        UpdateDotsPositions();
    }

    public override void _Draw()
    {
        UpdateDotsPositions();
    }

    public void OnScaleChanged()
    {
        QueueRedraw();
    }
}
