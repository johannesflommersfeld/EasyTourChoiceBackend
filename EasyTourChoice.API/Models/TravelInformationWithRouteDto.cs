using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Models;

public class TravelInformationWithRouteDto : TravelInformationDto
{
    public List<LocationDto>? Route; // should only be filled when requested to show on screen
}
