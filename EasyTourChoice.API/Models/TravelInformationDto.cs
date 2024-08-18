using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Models;

public class TravelInformationDto
{
    public required int TargetLocationId;
    public required Location StartingLocation;
    public required float TravelTime; // traveling time with the car in hours
}
