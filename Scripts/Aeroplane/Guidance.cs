using Godot;

namespace Guidance
{
    public enum TurnDirection
    {
        Left,
        Right,
        Quickest
    }

    public interface ILateralMode
    {
        float RollCommand() => 0;
        ILateralMode NewMode() => null;
    }

    public interface IVerticalMode
    {
        float FlightPathAngleCommand() => 0;
        IVerticalMode NewMode() => null;
    }

    public interface IArmableVerticalMode : IVerticalMode
    {
        bool Activate() => false;
    }

    public class HeadingSelect : ILateralMode
    {
        private readonly Aeroplane _aeroplane;
        public readonly float Heading;
        public readonly TurnDirection TurnDirection;

        private float HeadingDelta(TurnDirection direction)
        {
            // Find the angle in degrees between the aircraft's current heading and the selected heading, assuming a turn in a given direction
            float headingDelta = direction == TurnDirection.Left ? _aeroplane.TrueHeading - Heading : Heading - _aeroplane.TrueHeading;
            if (headingDelta < 0)
            {
                headingDelta += 360;
            }
            return headingDelta;
        }

        public float RollCommand()
        {
            // If the difference between the current and selected heading is greater than that needed to roll out,
            // command a standard rate turn in the appropriate direction
            if (HeadingDelta(TurnDirection) > (_aeroplane.Roll / Aeroplane.RollRate) * _aeroplane.Roll + 0.01f)
            {
                return TurnDirection == TurnDirection.Right ? Aeroplane.StandardRateTurn : -Aeroplane.StandardRateTurn;
            }
            else
            {
                return 0f;
            }
        }

        public HeadingSelect(Aeroplane aeroplane, float heading, TurnDirection turnDirection)
        {
            _aeroplane = aeroplane;
            Heading = heading;
            if (turnDirection == TurnDirection.Quickest)
            {
                // Find which direction it would be quicker to turn in to reach the selected heading
                TurnDirection = HeadingDelta(TurnDirection.Left) < HeadingDelta(TurnDirection.Right) ? TurnDirection.Left : TurnDirection.Right;
            }
            else
            {
                TurnDirection = turnDirection;
            }
        }
    }

    public class AltitudeHold : IArmableVerticalMode
    {
        private readonly Aeroplane _aeroplane;

        public bool Activate()
        {
            // If the difference between the selected altitude and the current altitude is less than that needed to level off, level off
            if (Mathf.Abs(_aeroplane.SelectedAltitude - _aeroplane.TrueAltitude) < Mathf.Abs(_aeroplane.FlightPathAngle) / Aeroplane.PitchRate * Mathf.Abs(_aeroplane.VerticalSpeed) / 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AltitudeHold(Aeroplane aeroplane) => _aeroplane = aeroplane;
    }

    public class VerticalSpeed : IVerticalMode
    {
        private readonly Aeroplane _aeroplane;
        public readonly float VerticalRate;

        public float FlightPathAngleCommand()
        {
            // Calculate the flight path angle required to maintain a given vertical speed at the current true airspeed
            float requiredAngle = Mathf.RadToDeg(Mathf.Asin(VerticalRate * Aeroplane.FeetPerMinuteToKnots / _aeroplane.TrueAirspeed));
            return _aeroplane.TrueAltitude < _aeroplane.SelectedAltitude ? requiredAngle : -requiredAngle;
        }

        public VerticalSpeed(Aeroplane aeroplane, float verticalSpeed)
        {
            _aeroplane = aeroplane;
            VerticalRate = verticalSpeed;
        }
    }
}