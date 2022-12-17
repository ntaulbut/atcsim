using Godot;

public partial class RangeRings : Node2D
{
    [Export] public float MinRadius = 5;
    [Export] public int NumberOfRings = 5;
    [Export] public float RingSpacing = 5;
    [Export] Color color;

    public override void _Ready()
    {
        GetNode<Simulator>("/root/Simulator").ScaleChanged += OnScaleChanged;
    }

    public override void _Draw()
    {
        GD.Print("Drawing range rings");
        //Vector2 PositionNm = Geo.RelativePositionNm(new Vector2((float)34.436, (float)132.919), Session.RadarConfig.LatLon);
        //Position = Session.ScaledPosition(PositionNm, GetViewportRect());
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