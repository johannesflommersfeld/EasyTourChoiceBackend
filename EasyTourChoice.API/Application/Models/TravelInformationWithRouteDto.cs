namespace EasyTourChoice.API.Application.Models;

public class TravelInformationWithRouteDto : TravelInformationDto
{
    public List<LocationDto>? Route; // should only be filled when requested to show on screen
}
