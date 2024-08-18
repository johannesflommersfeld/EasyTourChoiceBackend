using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Services;

public interface ITravelPlanningService
{
    Task<TravelInformationDto> GetShortTravelInfoAsync(Location currentLocation, Location targetLocation);
    Task<TravelInformationWithRouteDto> GetLongTravelInfoAsync(Location currentLocation, Location targetLocation);
}