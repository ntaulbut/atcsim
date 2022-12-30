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
    }

    public class LateralMode : Mode
    {
        public virtual float RollCommand()
        {
            return 0f;
        }
    }

    public class HeadingSelect : LateralMode
    {
        private float _selectedHeading;
        private TurnDirection _turnDirection;

        private float HeadingDelta(TurnDirection direction)
        {
            // Find the angle in degrees between the aircraft's current heading and the selected heading, assuming a turn in a given direction.
            float headingDelta = direction == TurnDirection.Left ? _aeroplane.TrueHeading - _selectedHeading : _selectedHeading - _aeroplane.TrueHeading;
            if (headingDelta < 0)
            {
                headingDelta += 360;
            }
            return headingDelta;
        }

        public override float RollCommand()
        {
            if (HeadingDelta(_turnDirection) > (_aeroplane.Roll / Aeroplane.RollRate) * _aeroplane.Roll + 0.01f)
            {
                return _turnDirection == TurnDirection.Right ? Aeroplane.StandardRateTurn : -Aeroplane.StandardRateTurn;
            }
            else
            {
                return 0f;
            }
        }

        public HeadingSelect(Aeroplane aeroplane, float heading, TurnDirection turnDirection)
        {
            _aeroplane = aeroplane;
            _selectedHeading = heading;
            if (turnDirection == TurnDirection.Quickest)
                _turnDirection = HeadingDelta(TurnDirection.Left) < HeadingDelta(TurnDirection.Right) ? TurnDirection.Left : TurnDirection.Right;
            else
                _turnDirection = turnDirection;
        }
    }
}