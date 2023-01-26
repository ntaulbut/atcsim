using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Tag : Control
{
    [Export] private Control TagDisplay;
    [Export] private PanelContainer TagDisplayPanelContainer;
    [Export] private Control _innerControlArea;

    [Export] private Control _headingField;
    [Export] private Control _altitudeField;

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
        // Detect change in hovering
        // Hover over inner rect to start hovering, but must move mouse out of entire
        // area to stop hovering
        bool nowHovering;
        if (Hovering)
        {
            nowHovering = TagDisplay.GetRect().HasPoint(GetLocalMousePosition());
        }
        else
        {
            nowHovering = new Rect2(TagDisplay.Position + _innerControlArea.GetRect().Position, _innerControlArea.GetRect().Size).HasPoint(GetLocalMousePosition());
        }

        // Stay hovering if any control in the tag is focussed
        nowHovering |= new Control[] { _headingField, _altitudeField }.Any(control => control.HasFocus());
        // Stay hovering if dragging
        nowHovering |= dragging;

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

        // Dragging
        // When first clicking on the tag, set the offset position of the mouse from the tag origin
        if (Hovering && Input.IsActionJustPressed("Drag Tag"))
        {
            dragging = true;
            offset = GetGlobalMousePosition() - TagDisplay.GlobalPosition;
        }
        if (Input.IsActionJustReleased("Drag Tag"))
        {
            dragging = false;
            savedGlobalPosition = TagDisplay.GlobalPosition;
        }

        // When dragging, set tag to mouse position
        if (dragging)
        {
            TagDisplay.GlobalPosition = GetGlobalMousePosition() - offset;
        }

        // When hovering over the tag but not dragging, freeze its position
        // if (Hovering && !dragging)
        //     TagDisplay.GlobalPosition = savedGlobalPosition;

        QueueRedraw();
    }

    public override void _Draw()
    {
        // Drawing the line from the blip to the tag

        // Get the inner rect, adjusting its position because its otherwise local to its parent
        Rect2 innerRect = _innerControlArea.GetRect();
        innerRect.Position += TagDisplay.Position;
        // Intersect with the outer rect when hovering and the inner rect when not
        Rect2 tagRect = Hovering ? TagDisplay.GetRect() : innerRect; 

        // Define start and end points as blip position and tag centre
        Vector2 end = innerRect.GetCenter(); //tagRect.GetCenter();
        Vector2 start = Position + Position.DirectionTo(end) * 10;

        // Define line from blip to tag centre
        float m = (start.y - end.y) / (start.x - end.x);
        float c = start.y - m * start.x;

        // Calculate intersection points for all four lines
        float x = innerRect.Position.x;
        Vector2 p_a = new(x, m * x + c);
        float y = innerRect.Position.y;
        Vector2 p_b = new((y - c) / m, y);
        x = innerRect.Position.x + tagRect.Size.x;
        Vector2 p_c = new(x, m * x + c);
        y = innerRect.Position.y + tagRect.Size.y;
        Vector2 p_d = new((y - c) / m, y);
        List<Vector2> points = new() { p_a, p_b, p_c, p_d };

        // Draw line from start to closest intersection point
        // Do not draw the line if the tag is over the blip
        if (!tagRect.HasPoint(start))
        {
            DrawLine(start, points.MinBy(point => point.DistanceSquaredTo(end - end.Normalized() * 10f)), Colors.White, 1, true);
        }
    }
}
