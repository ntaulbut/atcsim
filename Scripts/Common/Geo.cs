using Godot;

public class Geo
{
    // Average radius of the Earth in nautical miles
    private const float EarthRadiusNm = 3438.175f;
    
    public static float Haversine(float x)
    {
        return Mathf.Pow(Mathf.Sin(x / 2f), 2);
    }

    public static float Archaversine(float x)
    {
        return 2f * Mathf.Asin(Mathf.Sqrt(x));
    }

    public static float GetDistanceNm(float latitude, float longitude, float otherLatitude, float otherLongitude)
    {
        // Get the distance in nautical miles between two points

        // Convert to radians
        float lat = Mathf.DegToRad(latitude);
        float lon = Mathf.DegToRad(longitude);
        float otherLat = Mathf.DegToRad(otherLatitude);
        float otherLon = Mathf.DegToRad(otherLongitude);
        // Use the haversine formula
        float haversineTheta = Haversine(otherLat - lat) + Mathf.Cos(lat) * Mathf.Cos(otherLat) * Haversine(otherLon - lon);
        return EarthRadiusNm * Archaversine(haversineTheta);
    }

    public static Vector2 RelativePositionNm(Vector2 position, Vector2 relativeTo)
    {
        // Get the position in nautical miles of a point relative to another

        float horizontalComponent = GetDistanceNm(relativeTo.X, relativeTo.Y, relativeTo.X, position.Y);
        horizontalComponent = position.Y < relativeTo.Y ? -horizontalComponent : horizontalComponent;

        float verticalComponent = GetDistanceNm(relativeTo.X, position.Y, position.X, position.Y);
        verticalComponent = position.X < relativeTo.X ? -verticalComponent : verticalComponent;

        return new Vector2(horizontalComponent, verticalComponent);
    }
}
