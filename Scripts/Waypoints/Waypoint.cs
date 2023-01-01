using Godot;
using System;

public partial class Waypoint : Sprite2D
{
    public WaypointData WaypointData;
    public Vector2 PositionNm;

    public override void _Ready()
    {
        // Set icon coressponding to waypoint type
        Texture = WaypointData.Basis switch
        {
            WaypointData.Type.RNAV => Simulator.RadarConfig.Style.RNAVTexture,
            WaypointData.Type.VOR => Simulator.RadarConfig.Style.VORTexture,
            WaypointData.Type.VORDME => Simulator.RadarConfig.Style.VORDMETexture,
            WaypointData.Type.NDB => Simulator.RadarConfig.Style.NDBTexture,
            _ => throw new NotImplementedException()
        };

        // Set name
        GetChild<Label>(0).Text = WaypointData.ResourceName;

        // Calculate position relative to screen centre
        PositionNm = Geo.RelativePositionNm(WaypointData.LatLon, Simulator.RadarConfig.LatLon);
    }

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
        Position = Simulator.ScaledPosition(PositionNm, GetViewportRect());
    }

    public void OnScaleChanged()
    {
        QueueRedraw();
    }
}
