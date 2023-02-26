using Godot;

public partial class Airport : Resource
{
    [Export] public ILSApproach[] ILSApproaches;
    [Export] public Runway[] Runways;
}
