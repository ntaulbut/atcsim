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
        screenVelocity = new Vector2(_aeroplane.GroundVector.x, -_aeroplane.GroundVector.y);
        Vector2 direction = screenVelocity.Normalized();
        Vector2 start = Position + direction * _start;
        Vector2 end = start + direction * _length;
        DrawLine(start, end, _colour, 1, true);
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public void DisplayUpdateTimeout()
    {
        // screenVelocity = new Vector2(_aeroplaneDisplay.Aeroplane.Velocity.x, -_aeroplaneDisplay.Aeroplane.Velocity.y);
    }

    public override void _Ready()
    {
        if (_colour.a < 0.1)
        {
            GD.PrintErr("Warning: Leader Line Transparent!");
        }
    }
}
