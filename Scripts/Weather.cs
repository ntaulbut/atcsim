using Godot;

public partial class Weather : Node
{
	[Export] public float WeatherChangeRate;

	public METAR metar;

	public override async void _Ready()
	{
		metar = await METAR.FromAWC(Simulator.RadarConfig.PrimaryICAOCode);
		Simulator.WindDirection = metar.WindDirection;
		Simulator.WindSpeed = metar.WindSpeed;
	}

	public override void _Process(double delta)
	{
		if (metar is not null)
		{
			Simulator.WindDirection = Mathf.MoveToward(Simulator.WindDirection, metar.WindDirection, WeatherChangeRate / 60 * (float)delta);
			Simulator.WindSpeed = Mathf.MoveToward(Simulator.WindSpeed, metar.WindSpeed, WeatherChangeRate / 60 * (float)delta);
		}
	}

	public void OnMETARUpdateTimeout()
	{
		metar.Update();
	}
}
