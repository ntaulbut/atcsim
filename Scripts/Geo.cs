using Godot;

public partial class Geo : Godot.Object
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
        float horizontalComponent = GetDistanceNm(relativeTo.x, relativeTo.y, relativeTo.x, position.y);
        horizontalComponent = position.y < relativeTo.y ? -horizontalComponent : horizontalComponent;

        float verticalComponent = GetDistanceNm(relativeTo.x, position.y, position.x, position.y);
        verticalComponent = position.x < relativeTo.x ? -verticalComponent : verticalComponent;

        return new Vector2(horizontalComponent, verticalComponent);
    }
}
