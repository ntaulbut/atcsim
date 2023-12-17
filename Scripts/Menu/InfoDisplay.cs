using Godot;

public partial class InfoDisplay : Label
{
	public override void _Process(double delta)
	{
		var timeDict = Time.GetTimeDictFromSystem(utc: true);
		string timeString = timeDict["hour"].ToString() + timeDict["minute"].ToString() + "Z";
		string windString = Mathf.Round(Simulator.WindDirection).ToString().PadLeft(3, '0') + "/" + Mathf.Round(Simulator.WindSpeed).ToString() + "kt";
		Text = Simulator.RadarConfig.PrimaryICAOCode + " " + timeString + " " + windString;
	}
}
