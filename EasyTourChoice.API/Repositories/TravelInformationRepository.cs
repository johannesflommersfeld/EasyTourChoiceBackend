using EasyTourChoice.API.Repositories.Interfaces;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EasyTourChoice.API.Repositories;

public class TravelInformationRepository(TourDataContext context): ITravelInformationRepository
{
    private readonly TourDataContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<TravelInformation?> GetTravelInformationAsync(Location startingLocation, int targetLocationId)
    {
        return await _context.TravelInformations
            .Include(t => t.StartingLocation)
            .FirstOrDefaultAsync(t =>
                t.StartingLocation != null && t.TargetLocation != null &&
                t.TargetLocation.LocationId == targetLocationId &&
                Math.Abs(Math.Round(t.StartingLocation.Latitude, LocationUtils.ROUND_PRECISION) -
                         startingLocation.Latitude) < Math.Pow(10, -LocationUtils.ROUND_PRECISION) &&
                Math.Abs(Math.Round(t.StartingLocation.Longitude, LocationUtils.ROUND_PRECISION) -
                         startingLocation.Longitude) < Math.Pow(10, -LocationUtils.ROUND_PRECISION)
            );
    }
    
    public async Task<bool> SaveTravelInformationAsync(TravelInformation travelInformation)
    {
        if (travelInformation.TargetLocationId is null || travelInformation.StartingLocation is null)
        {
            return false;
        }
        var existingInfo = await GetTravelInformationAsync(travelInformation.StartingLocation, (int)travelInformation.TargetLocationId);
        if (existingInfo is null)
        {
            await _context.TravelInformations.AddAsync(travelInformation);
        }
        else
        {
            existingInfo.TravelDistance = travelInformation.TravelDistance;
            existingInfo.TravelTime = travelInformation.TravelTime;
            existingInfo.Route = travelInformation.Route;
        }
        return await _context.SaveChangesAsync() >= 0;
    }
}