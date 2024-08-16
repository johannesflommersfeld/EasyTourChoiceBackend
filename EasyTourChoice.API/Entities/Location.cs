namespace EasyTourChoice.API.Entities;

public class Location
{
    public double? Latitude { get; init; } // latitude with decimal minutes
    public double? Longitude { get; init; } // longitude with decimal minutes
    public double? Altitude { get; init; } // altitude in meter

    private const double minLatitude = -180;
    private const double maxLatitude = 180;
    private const double minLongitude = -90;
    private const double maxLongitude = 90;
    private const double minAltitude = -450;
    private const double maxAltitude = 8_900;

    public Location(double? latitude, double? longitude, double? altitude = null)
    {

        if ((latitude != null && double.IsNaN((double)latitude)) ||
            (longitude != null && double.IsNaN((double)longitude)) ||
            (altitude != null && double.IsNaN((double)altitude)))
        {
            throw new ArgumentException("Nan is not a valid coordinate.");
        }

        if (latitude > maxLatitude || latitude < minLatitude ||
            longitude > maxLongitude || longitude < minLongitude ||
            altitude > maxAltitude || altitude < minAltitude)
        {
            var msg = string.Format("Longitude has to be in the range [{0}, {1}], "
                + "latitude has to be in the range [{2}, {3}], "
                + "altitude has to be in the range [{4}, {5}].",
                minLongitude, maxLongitude, minLatitude, maxLatitude, minAltitude, maxAltitude);

            throw new ArgumentException(msg);
        }

        Latitude = latitude;
        Longitude = longitude;
        Altitude = altitude;
    }
}