using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class HistoryDots : Node2D
{
    [Export] public Aeroplane Aeroplane;

    private List<Vector2> _previousPositions = new();
    private Vector2 _previousPosition;

    private void AddDot()
    {
        // Instantiate a new dot sprite with the relevant style
        RadarStyle style = Simulator.RadarConfig.Style;
        Sprite2D dot = new()
        {
            Texture = style.HistoryDotTexture,
            // Pick a colour from the given gradient depending on its position in the series of dots
            Modulate = style.HistoryDotsColourGradient.Sample(GetChildCount() / (float)style.HistoryDotsCount)
        };
        AddChild(dot);
    }

    private void UpdateDotsPositions()
    {
        // Update the position of all the dots
        for (int i = 0; i < GetChildCount(); i++)
        {
            GetChild<Sprite2D>(i).Position = Simulator.ScaledPosition(_previousPositions[i], GetViewportRect());
        }
    }

    private void Update()
    {
        // Keep track of previous positions of the aircraft
        _previousPosition = Aeroplane.PositionNm;
        _previousPositions.Insert(0, _previousPosition);

        // As there are more previous positions, introduce more dots up to the maximum
        if (GetChildCount() <= Simulator.RadarConfig.Style.HistoryDotsCount)
        {
            AddDot();
        }

        UpdateDotsPositions();
    }

    public void DisplayUpdateTimeout()
    {
        Update();
    }

    public override void _Ready()
    {
        Update();
    }

    public override void _Draw()
    {
        UpdateDotsPositions();
    }

    public void OnScaleChanged()
    {
        QueueRedraw();
    }

    public override void _EnterTree()
    {
        GetNode<Simulator>("/root/Simulator").ScaleChanged += OnScaleChanged;
    }

    public override void _ExitTree()
    {
        GetNode<Simulator>("/root/Simulator").ScaleChanged -= OnScaleChanged;
    }
}