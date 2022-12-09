using Godot;
using System;

public partial class Util : Godot.Object
{
    public static Vector2 HeadingToVector(float heading)
    {
        // Take a heading between 0-360 degrees and convert it to a normalized vector in that direction
        Vector2 quadrant = new Vector2(1, 1);
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
}
