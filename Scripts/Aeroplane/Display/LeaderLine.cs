using Godot;

public partial class LeaderLine : Node2D
{
    [Export] private Aeroplane _aeroplane;

    [Export] private Color _colour;
    [Export] private int _start = 10;
    [Export] private int _length = 40;

    private Vector2 screenVelocity;

    public override void _Draw()
    {
        // Invert the y-axis of the aeroplane's movement because the display uses an inverted y-axis
        screenVelocity = new Vector2(_aeroplane.GroundVector.X, -_aeroplane.GroundVector.Y);
        // Draw the line
        Vector2 direction = screenVelocity.Normalized();
        Vector2 start = Position + direction * _start;
        Vector2 end = start + direction * _length;
        DrawLine(start, end, _colour, -1, true);
    }

    public void DisplayUpdateTimeout()
    {
        QueueRedraw();
    }

    public void OnAeroplaneReady()
    {
        if (_colour.A < 0.1)
        {
            GD.PrintErr("Warning: Leader Line Transparent!");
        }
        QueueRedraw();
    }
}
