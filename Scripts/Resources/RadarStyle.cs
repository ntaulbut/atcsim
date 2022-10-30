using Godot;
using System;

public partial class RadarStyle : Resource
{
    [Export] public Texture2D RNAVTexture;
    [Export] public Texture2D VORDMETexture;
    [Export] public Texture2D VORTexture;
    [Export] public Texture2D NDBTexture;
    [Export] public Color CoastlineColour;
}
