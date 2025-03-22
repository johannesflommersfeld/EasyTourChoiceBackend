namespace EasyTourChoice.API.Domain;

public static class LocationUtils
{
    public const int ROUND_PRECISION = 1;
    
    public static Location RoundLocation(Location location)
    {
        return new Location()
        {
            LocationId = location.LocationId,
            Longitude = Math.Round(location.Longitude, ROUND_PRECISION),
            Latitude = Math.Round(location.Latitude, ROUND_PRECISION),
            Altitude = location.Altitude is null ? null : Math.Round((double)location.Altitude, ROUND_PRECISION),
        };
    }
}