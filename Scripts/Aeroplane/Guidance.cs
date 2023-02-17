using Godot;
using static Geo;

namespace Guidance
{
    public class PIDController
    {
        private readonly float _kp;
        private readonly float _ki;
        private readonly float _kd;
        private float previousError;
        private float _integral;

        public float Iterate(float delta, float setPoint, float measuredValue)
        {
            float error = setPoint - measuredValue;
            if (previousError != 0)
            {
                _integral += error * delta;
                float derivative = (error - previousError) / delta;
                previousError = error;
                return _kp * error + _ki * _integral + _kd * derivative;
            }
            else
            {
                previousError = error;
                return 0;
            }
        }

        public PIDController(float kp, float ki, float kd)
        {
            _kp = kp;
            _ki = ki;
            _kd = kd;
        }
    }

    public enum TurnDirection
    {
        Left,
        Right,
        Quickest
    }

    public interface ILateralMode
    {
        float RollCommand(float delta) => 0;
        ILateralMode NewMode() => null;
    }

    public interface IArmableLateralMode : ILateralMode
    {
        bool Activate() => false;
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
        public readonly TurnDirection TurnDirection;

        private float HeadingDelta(TurnDirection direction)
        {
            // Find the angle in degrees between the aircraft's current heading and the selected heading, assuming a turn in a given direction
            float headingDelta = direction == TurnDirection.Left ? _aeroplane.TrueHeading - _aeroplane.SelectedHeading : _aeroplane.SelectedHeading - _aeroplane.TrueHeading;
            if (headingDelta < 0)
            {
                headingDelta += 360;
            }
            return headingDelta;
        }

        public float RollCommand(float _)
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

        public HeadingSelect(Aeroplane aeroplane, TurnDirection turnDirection)
        {
            _aeroplane = aeroplane;
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

    public class Localiser : IArmableLateralMode
    {
        private readonly Aeroplane _aeroplane;
        private readonly PIDController _controller = new(2, 0, 150);

        private float Deviation()
        {
            ILSApproach approach = _aeroplane.Approach;
            Vector2 locPosition = RelativePositionNm(approach.LocaliserLatLon, Simulator.RadarConfig.LatLon);
            float distance = _aeroplane.PositionNm.DistanceTo(locPosition);
            float deviation = distance * Mathf.Sin(2 * Mathf.Pi - Mathf.DegToRad(Util.OppositeHeading(approach.LocaliserHeading)) - Mathf.Asin(Mathf.Abs(_aeroplane.PositionNm.X - locPosition.X) / distance));
            return _aeroplane.PositionNm.Y - locPosition.Y > 0 ? -deviation : deviation;
        }
        
        public bool Activate()
        {
            return Mathf.Abs(Deviation()) < 0.25;
        }

        public float RollCommand(float delta)
        {
            return _controller.Iterate(delta, 0, Deviation());
        }

        public Localiser(Aeroplane aeroplane) => _aeroplane = aeroplane;
    }

    public class Glideslope : IArmableVerticalMode
    {
        private readonly Aeroplane _aeroplane;

        private float Deviation()
        {
            ILSApproach approach = _aeroplane.Approach;
            float distance = _aeroplane.PositionNm.DistanceTo(RelativePositionNm(approach.GlideslopeLatLon, Simulator.RadarConfig.LatLon));
            float glideslopeHeight = distance * Aeroplane.NauticalMilesToFeet * Mathf.Tan(Mathf.DegToRad(approach.GlideslopeAngle)) + approach.GlideslopeElevation;
            return _aeroplane.TrueAltitude - glideslopeHeight;
        }

        public bool Activate()
        {
            ILSApproach approach = _aeroplane.Approach;
            float pitchTime = (_aeroplane.FlightPathAngle + approach.GlideslopeAngle) / Aeroplane.PitchRate * _aeroplane.TrueAirspeed * 0.845f;
            float deviation = Deviation();
            return Mathf.Abs(deviation) * (1 / Mathf.Tan(Mathf.DegToRad(approach.GlideslopeAngle))) < pitchTime && deviation < 0;
        }

        public float FlightPathAngleCommand()
        {
            return -_aeroplane.Approach.GlideslopeAngle;
        }

        public Glideslope(Aeroplane aeroplane) => _aeroplane = aeroplane;
    }
}