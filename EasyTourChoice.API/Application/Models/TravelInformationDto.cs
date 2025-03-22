using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Application.Models;

public class TravelInformationDto
{
    public required int? TargetLocationId;
    public required Location StartingLocation;
    public required float TravelTime; // traveling time with the car in hours
    public required float TravelDistance; // traveling time with the car in hours
}
