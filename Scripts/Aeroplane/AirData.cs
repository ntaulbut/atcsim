using Godot;

public class AirData
{
    public const float KnotsToMs = 0.5144444444f;

    public const float R = 8.31446261815324f; // J/(mol·K) (Universal gas constant)
    public const float gamma = 1.4f;
    public const float g = 9.80665f; // m/s^2 (Gravitational acceleration)
    public const float M = 0.0289644f; // kg/mol (Molar mass of Earth's air)

    // Assuming the Troposphere
    // not correct above 11,000 metres
    public const float P = 101325.00f; // Pascals (Reference pressure)
    public const float T = 288.15f; // Kelvin (Reference temperature)
    // note: use T from METAR in future?
    public const float L = -0.0065f; // Kelvin / Metre (Reference temperature lapse rate)
    public const float hb = 0; //Metres (Height of reference level)

    public static float Temperature(float height)
    {
        // height: metres, returns: kelvin
        return T + (height - hb) * L;
    }

    public static float SpeedOfSound(float height)
    {
        // height: metres, speed: metres/second
        return Mathf.Sqrt(gamma * (R / M) * TemperatureAt(height));
    }

    public static float Mach(float trueAirspeed, float height)
    {
        // https://en.wikipedia.org/wiki/Mach_number#Calculation
        // trueAirspeed: knots, height: metres, returns: metres/second
        return trueAirspeed * KnotsToMs / SpeedOfSoundAt(height);
    }

    public static float Pressure(float height)
    {
        // https://en.wikipedia.org/wiki/Barometric_formula
        // height: metres, returns: hectopascals
        return P * Mathf.Pow(TemperatureAt(height) / T, (-g * M) / (R * L)) / 100;
    }

    public static float PressureAltitude(float height)
    {
        // https://en.wikipedia.org/wiki/Pressure_altitude
        // height: metres, returns: feet
        return (1 - Mathf.Pow(PressureAt(height) / 1013.25f, 0.190284f)) * 145366.45f;
    }

    public static float IndicatedAltitude(float height, float referencePressure)
    {
        // height: metres, referencePressure: hectopascals, returns: feet
        return (1 - Mathf.Pow(PressureAt(height) / referencePressure, 0.190284f)) * 145366.45f;
    }
}