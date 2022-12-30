using Godot;

public partial class DebugText : Label
{
    [Export] public Aeroplane Aeroplane;

    public override void _Process(double delta)
	{
        float height = Aeroplane.TrueAltitude;
        Text = string.Format("TAS: {0}kts\nGS: {1}kts\nT: {2}K/{3}C\nSpeed of Sound: {4}m/s\nMach: {5}\nPress: {6}hPa\nPressAlt: {7}ft",
            Aeroplane.TrueAirspeed,
            Aeroplane.GroundVector.Length(),
            AirData.Temperature(height),
            AirData.Temperature(height) - 273,
            AirData.SpeedOfSound(height),
            AirData.Mach(Aeroplane.TrueAirspeed, height),
            AirData.Pressure(height),
            AirData.PressureAltitude(height));
	}
}
