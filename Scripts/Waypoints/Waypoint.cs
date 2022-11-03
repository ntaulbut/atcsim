using Godot;
using System;

public partial class Waypoint : Sprite2D
{
    public WaypointData WaypointData;
    public RadarConfig RadarConfig;
    public Vector2 PositionNm;

    public override void _Ready()
    {
        // Set icon coressponding to waypoint type
        Texture = WaypointData.Basis switch
        {
            WaypointData.Type.RNAV => RadarConfig.Style.RNAVTexture,
            WaypointData.Type.VOR => RadarConfig.Style.VORTexture,
            WaypointData.Type.VORDME => RadarConfig.Style.VORDMETexture,
            WaypointData.Type.NDB => RadarConfig.Style.NDBTexture,
            _ => throw new NotImplementedException()
        };

        // Set name
        GetChild<Label>(0).Text = WaypointData.ResourceName;

        // Calculate position relative to screen centre
        PositionNm = Geo.RelativePositionNm(WaypointData.LatLon, RadarConfig.LatLon);
    }

    public override void _Draw()
    {
        Position = RadarConfig.ScaledPosition(PositionNm, GetViewportRect());
    }
}
