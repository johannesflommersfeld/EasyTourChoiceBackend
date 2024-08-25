using System.Text.Json.Serialization;

namespace EasyTourChoice.API.Models.BaseModels;

public class LocationBase : IEquatable<object>
{
    [property: JsonPropertyName("latitude")]
    public double Latitude { get; set; } // latitude with decimal minutes

    [property: JsonPropertyName("longitude")]
    public double Longitude { get; set; } // longitude with decimal minutes
    public double? Altitude { get; set; } // altitude in meter

    // according to https://en.wikipedia.org/wiki/Decimal_degrees,
    // this should be precise enough for our purposes (+/- 10 m)
    private const double _locationTolerance = 0.0001;

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        
        var loc = (LocationBase)obj;
        return Math.Abs(Latitude - loc.Latitude) < _locationTolerance && 
               Math.Abs(Longitude - loc.Longitude) < _locationTolerance; 
    }

    public static bool operator == (LocationBase? loc1, LocationBase? loc2)
    {
        if (loc1 is null)
            return loc2 is null;

        return loc1.Equals(loc2);
    }

    public static bool operator != (LocationBase? loc1, LocationBase? loc2)
    {
        return !(loc1 == loc2);
    }

    public override int GetHashCode()
    {
        return Latitude.GetHashCode() ^ Longitude.GetHashCode() ^ Altitude.GetHashCode();
    }
}