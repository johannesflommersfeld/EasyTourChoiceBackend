namespace EasyTourChoice.API.Entities;

public class Location
{
    public double? Latitude { get; init; } // latitude with decimal minutes
    public double? Longitude { get; init; } // longitude with decimal minutes
    public double? Altitude { get; init; } // altitude in meter
}