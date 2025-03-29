using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Application.DataAggregation;

public class TravelPlanningServiceTomTom : ITravelPlanningService
{
    public Task<TravelInformationDto?> GetShortTravelInfoAsync(Location currentLocation, Location targetLocation)
    {
        throw new NotImplementedException();
    }

    public Task<TravelInformationWithRouteDto?> GetLongTravelInfoAsync(Location currentLocation, Location targetLocation)
    {
        throw new NotImplementedException();
    }
}