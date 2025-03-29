using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories.Interfaces;

public interface ITravelInformationRepository
{
    public Task<bool> SaveTravelInformationAsync(TravelInformation travelInformation);
    public Task<TravelInformation?> GetTravelInformationAsync(Location startLocation, int targetLocationId);
}