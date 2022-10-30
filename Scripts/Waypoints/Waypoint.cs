using Godot;
using System;

public partial class Waypoint : Sprite2D
{
    [Export] public WaypointData WaypointData;
    [Export] public Vector2 PositionNm;

    public override void _Ready()
    {
        RadarConfig radarConfig = Session.RadarConfig;

        // Set icon coressponding to waypoint type
        Texture = WaypointData.Basis switch
        {
            WaypointData.Type.RNAV => radarConfig.Style.RNAVTexture,
            WaypointData.Type.VOR => radarConfig.Style.VORTexture,
            WaypointData.Type.VORDME => radarConfig.Style.VORDMETexture,
            WaypointData.Type.NDB => radarConfig.Style.NDBTexture,
            _ => throw new NotImplementedException()
        };

        // Set name
        GetChild<Label>(0).Text = WaypointData.ResourceName;

        // Calculate position relative to screen centre
        PositionNm = Geo.RelativePositionNm(WaypointData.LatLon, radarConfig.LatLon);
    }

    public override void _Draw()
    {
        Position = Session.ScaledPosition(PositionNm, GetViewportRect());
    }
}
