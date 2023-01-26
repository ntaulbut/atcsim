using Godot;

namespace Guidance
{
    public enum TurnDirection
    {
        Left,
        Right,
        Quickest
    }

    public class Mode
    {
        protected Aeroplane _aeroplane;

        public Mode(Aeroplane aeroplane)
        {
            _aeroplane = aeroplane;
        }
    }

    public class LateralMode : Mode
    {
        public LateralMode(Aeroplane aeroplane) : base(aeroplane) { }

        public virtual float RollCommand()
        {
            return 0f;
        }

        public virtual LateralMode NewMode()
        {
            return null;
        }
    }

    public class VerticalMode : Mode
    {
        public VerticalMode(Aeroplane aeroplane) : base(aeroplane) { }

        public virtual float FlightPathAngleCommand()
        {
            return 0f;
        }

        public virtual VerticalMode NewMode()
        {
            return null;
        }
    }

    public class HeadingSelect : LateralMode
    {
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

        public override float RollCommand()
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

        public HeadingSelect(Aeroplane aeroplane, float heading, TurnDirection turnDirection) : base(aeroplane)
        {
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

    public class AltitudeHold : VerticalMode
    {
        public AltitudeHold(Aeroplane aeroplane) : base(aeroplane) { }
    }

    public class VerticalSpeed : VerticalMode
    {
        public readonly float Altitude;
        public readonly float VerticalRate;

        public override float FlightPathAngleCommand()
        {
            // calculate the flight path angle required to maintain a given vertical speed at the current true airspeed
            float requiredAngle = Mathf.RadToDeg(Mathf.Asin(VerticalRate * Aeroplane.FeetPerMinuteToKnots / _aeroplane.TrueAirspeed));
            return _aeroplane.TrueAltitude < Altitude ? requiredAngle : -requiredAngle;
        }

        public override VerticalMode NewMode()
        {
            if (Mathf.Abs(Altitude - _aeroplane.TrueAltitude) < Mathf.Abs(_aeroplane.FlightPathAngle) / Aeroplane.PitchRate * Mathf.Abs(_aeroplane.VerticalSpeed) / 2)
            {
                return new AltitudeHold(_aeroplane);
            }
            else
            {
                return null;
            }
        }

        public VerticalSpeed(Aeroplane aeroplane, float altitude, float verticalSpeed) : base(aeroplane)
        {
            Altitude = altitude;
            VerticalRate = verticalSpeed;
        }
    }
}