using Godot;

public partial class RangeRings : Node2D
{
    [Export] public float MinRadius = 5;
    [Export] public int NumberOfRings = 5;
    [Export] public float RingSpacing = 5;
    [Export] Color color;

    public override void _EnterTree()
    {
        GetNode<Simulator>("/root/Simulator").ScaleChanged += OnScaleChanged;
    }

    public override void _ExitTree()
    {
        GetNode<Simulator>("/root/Simulator").ScaleChanged -= OnScaleChanged;
    }

    public override void _Draw()
    {
        GD.Print("Drawing range rings");
        // Draw the specified number of rings starting with the minimum radius and increasing by the given spacing
        for (int i = 0; i < NumberOfRings; i++)
        {
            float radius = (MinRadius + i * RingSpacing) * Simulator.Scale(GetViewportRect());
            DrawArc(Vector2.Zero, radius, 0, 2 * Mathf.Pi, (int)(2 * Mathf.Pi * radius), color);
        }
    }

    public void OnScaleChanged()
    {
        QueueRedraw();
    }
}
