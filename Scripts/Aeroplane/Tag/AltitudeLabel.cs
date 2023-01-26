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
        Text = Mathf.RoundToInt(Aeroplane.TrueAltitude / 100).ToString().PadLeft(3, '0');

        if (Aeroplane.VerticalSpeed * 60 > _arrowFPMDeadzone)
        {
            _arrowLabel.Text = _upArrowChar;
        }
        else if (Aeroplane.VerticalSpeed * 60 < -_arrowFPMDeadzone)
        {
            _arrowLabel.Text = _downArrowChar;
        }
        else
        {
            _arrowLabel.Text = "";
        }
    }
}
