using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Controllers.Interfaces;

public interface ITravelPlanningService
{
    Task<TravelInformationDto?> GetShortTravelInfoAsync(Location currentLocation, Location targetLocation);
    Task<TravelInformationWithRouteDto?> GetLongTravelInfoAsync(Location currentLocation, Location targetLocation);
}