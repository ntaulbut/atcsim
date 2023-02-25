using Godot;

public partial class AltitudeLabel : Label
{
    [Export] public Aeroplane Aeroplane;
    [Export] private Label _arrowLabel;
    [Export] private string _upArrowChar;
    [Export] private string _downArrowChar;
    [Export] private int _arrowFPMDeadzone;

    public override void _Ready()
	{
        Update();
    }

    public void ADSB2HzTimeout()
    {
        Update();
    }

    private void Update()
    {
        // Display the current altitude in hundreds of feet
        Text = Mathf.RoundToInt(Aeroplane.TrueAltitude / 100).ToString().PadLeft(3, '0');

        // Note the aeroplane vertical speed is in feet/s and _arrowFPMDeadzone is in fpm

        // If the vertical speed is positive with a magnitude greater than the specified margin, show the climb indicator
        if (Aeroplane.VerticalSpeed * 60 > _arrowFPMDeadzone)
        {
            _arrowLabel.Text = _upArrowChar;
        }
        // If the vertical speed is negative with a magnitude greater than the specified margin, show the descent indicator
        else if (Aeroplane.VerticalSpeed * 60 < -_arrowFPMDeadzone)
        {
            _arrowLabel.Text = _downArrowChar;
        }
        else
        {
            // Show nothing
            _arrowLabel.Text = "";
        }
    }
}
