using Godot;

public class Util
{
    public static Vector2 HeadingToVector(float heading)
    {
        // Take a heading between 0-360 degrees and convert it to a normalized vector in that direction
        Vector2 quadrant = new(1, 1);
        float theta = heading;
        if (heading > 270)
        {
            theta = 360 - heading;
            quadrant = new Vector2(-1, 1);
        }
        else if (heading > 180)
        {
            theta = heading - 180;
            quadrant = new Vector2(-1, -1);
        }
        else if (heading > 90)
        {
            theta = 180 - heading;
            quadrant = new Vector2(1, -1);
        }
        return new Vector2(Mathf.Sin(Mathf.DegToRad(theta)), Mathf.Sin(Mathf.DegToRad(90 - theta))) * quadrant;
    }

    public static float Bearing(Vector2 from, Vector2 to)
    {
        // Calculate the absolute bearing from one point to another
        float theta = Mathf.RadToDeg(Mathf.Asin(Mathf.Abs(from.X - to.X) / from.DistanceTo(to)));
        if (Mathf.Sign(from.X - to.X) == 1)
        {
            if (Mathf.Sign(from.Y - to.Y) == 1)
            {
                return 180 + theta;
            }
            else
            {
                return 360 - theta;
            }
        }
        else
        {
            if (Mathf.Sign(from.Y - to.Y) == 1)
            {
                return 180 - theta;
            }
            else
            {
                return theta;
            }
        }
    }

    public static float OppositeHeading(float heading)
    {
        return heading <= 180 ? heading + 180 : heading - 180;
    }
}
