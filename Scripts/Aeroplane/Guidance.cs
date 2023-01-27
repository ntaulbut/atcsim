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

    public class HeadingSelect : ILateralMode
    {
        private readonly Aeroplane _aeroplane;
        public readonly float Heading;
        public readonly TurnDirection TurnDirection;

        private float HeadingDelta(TurnDirection direction)
        {
            // Find the angle in degrees between the aircraft's current heading and the selected heading, assuming a turn in a given direction.
            float headingDelta = direction == TurnDirection.Left ? _aeroplane.TrueHeading - Heading : Heading - _aeroplane.TrueHeading;
            if (headingDelta < 0)
            {
                headingDelta += 360;
            }
            return headingDelta;
        }

        public float RollCommand()
        {
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
                TurnDirection = HeadingDelta(TurnDirection.Left) < HeadingDelta(TurnDirection.Right) ? TurnDirection.Left : TurnDirection.Right;
            }
            else
            {
                TurnDirection = turnDirection;
            }
        }
    }

    public class AltitudeHold : IVerticalMode
    {

    }

    public class VerticalSpeed : IVerticalMode
    {
        private readonly Aeroplane _aeroplane;
        public readonly float Altitude;
        public readonly float VerticalRate;

        public float FlightPathAngleCommand()
        {
            // calculate the flight path angle required to maintain a given vertical speed at the current true airspeed
            float requiredAngle = Mathf.RadToDeg(Mathf.Asin(VerticalRate * Aeroplane.FeetPerMinuteToKnots / _aeroplane.TrueAirspeed));
            return _aeroplane.TrueAltitude < Altitude ? requiredAngle : -requiredAngle;
        }

        public IVerticalMode NewMode()
        {
            if (Mathf.Abs(Altitude - _aeroplane.TrueAltitude) < Mathf.Abs(_aeroplane.FlightPathAngle) / Aeroplane.PitchRate * Mathf.Abs(_aeroplane.VerticalSpeed) / 2)
            {
                return new AltitudeHold();
            }
            else
            {
                return null;
            }
        }

        public VerticalSpeed(Aeroplane aeroplane, float altitude, float verticalSpeed)
        {
            _aeroplane = aeroplane;
            Altitude = altitude;
            VerticalRate = verticalSpeed;
        }
    }
}