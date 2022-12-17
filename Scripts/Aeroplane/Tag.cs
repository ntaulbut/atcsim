using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Tag : Control
{
    [Export] private Control TagDisplay;
    [Export] private PanelContainer TagDisplayPanelContainer;
    [Export] private Control _innerControlArea;
    
    public bool Hovering = false;
    private Vector2 savedGlobalPosition;
    private bool dragging = false;
    private Vector2 offset;

    public override void _Ready()
    {
        TagDisplayPanelContainer.AddThemeStyleboxOverride("panel", Simulator.RadarConfig.Style.TagPanelNormal);
    }

    public override void _Process(double delta)
    {
        // When dragging, set tag to mouse position
        if (dragging)
            TagDisplay.GlobalPosition = GetGlobalMousePosition() - offset;

        // When hovering over the tag but not dragging, freeze its position
        if (Hovering && !dragging)
            TagDisplay.GlobalPosition = savedGlobalPosition;

        // Detect change in hovering
        // Hover over inner rect to start hovering, but must move mouse out of entire
        // area to stop hovering
        bool nowHovering;
        if (Hovering)
            nowHovering = TagDisplay.GetRect().HasPoint(GetLocalMousePosition());
        else
            nowHovering = new Rect2(TagDisplay.Position + _innerControlArea.GetRect().Position, _innerControlArea.GetRect().Size).HasPoint(GetLocalMousePosition());
            
        // Starting hovering
        if (!Hovering && nowHovering)
        {
            TagDisplayPanelContainer.AddThemeStyleboxOverride("panel", Simulator.RadarConfig.Style.TagPanelHovered);
            savedGlobalPosition = TagDisplay.GlobalPosition;
        }
        // Stopping hovering
        if (Hovering && !nowHovering)
        {
            // To account for the tag lagging behind the mouse, always keep background if dragging
            if (!dragging)
            {
                TagDisplayPanelContainer.AddThemeStyleboxOverride("panel", Simulator.RadarConfig.Style.TagPanelNormal);
            }
        }
        Hovering = nowHovering;

        QueueRedraw();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton)
        {
            // Left Mouse Button Pressed
            if (eventMouseButton.ButtonIndex == MouseButton.Left && eventMouseButton.Pressed)
            {
                // When first clicking on the tag, set the offset position of the mouse from the tag origin
                if (Hovering)
                {
                    offset = GetGlobalMousePosition() - TagDisplay.GlobalPosition;
                    dragging = true;
                }
            }
            // Left Mouse Button Released
            if (eventMouseButton.ButtonIndex == MouseButton.Left && !eventMouseButton.Pressed)
            {
                dragging = false;
                savedGlobalPosition = TagDisplay.GlobalPosition;
            }
        }       
    }

    public override void _Draw()
    {
        // Drawing the line from the blip to the tag

        Rect2 tagRect = TagDisplay.GetRect();

        // Define start and end points as blip position and tag centre
        Vector2 end = TagDisplay.Position + TagDisplay.Size / 2;
        Vector2 start = Position + Position.DirectionTo(end) * 10;

        // Define line from blip to tag centre
        float m = (start.y - end.y) / (start.x - end.x);
        float c = start.y - m * start.x;

        // Calculate intersection points for all four lines
        float x = tagRect.Position.x;
        Vector2 p_a = new Vector2(x, m * x + c);
        float y = tagRect.Position.y;
        Vector2 p_b = new Vector2((y - c) / m, y);
        x = tagRect.Position.x + tagRect.Size.x;
        Vector2 p_c = new Vector2(x, m * x + c);
        y = tagRect.Position.y + tagRect.Size.y;
        Vector2 p_d = new Vector2((y - c) / m, y);

        // Find the point closest to the centre of the tag
        List<Vector2> points = new List<Vector2> { p_a, p_b, p_c, p_d };
        Dictionary<float, Vector2> distances = points.ToDictionary(point => point.DistanceSquaredTo(end - end.Normalized() * 0.1f));

        // Draw line from start to closest intersection point
        DrawLine(start, distances[distances.Keys.Min()], Colors.White, 1, true);
    }
}
