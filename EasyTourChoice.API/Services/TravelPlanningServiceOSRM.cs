using EasyTourChoice.API.Entities;
using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Services;

public class OSRMTravelPlanningService : ITravelPlanningService
{
    public Task<TravelInformationDto> GetShortTravelInfoAsync(Location currentLocation, Location targetLocation)
    {
        throw new NotImplementedException();
    }

    public Task<TravelInformationWithRouteDto> GetLongTravelInfoAsync(Location currentLocation, Location targetLocation)
    {
        throw new NotImplementedException();
    }
}