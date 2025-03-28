using Microsoft.EntityFrameworkCore;
using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Repositories.Interfaces;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories;

public class LocationRepository(TourDataContext context) : ILocationRepository
{
    private readonly TourDataContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<bool> LocationExistsAsync(int id)
    {
        return await _context.Locations.AnyAsync(l => l.LocationId == id);
    }

    public async Task<Location?> GetLocationAsync(int id)
    {
        return await _context.Locations.Where(l => l.LocationId == id).FirstOrDefaultAsync();
    }

    public async Task<int?> FindLocationIdAsync(Location location)
    {
        var locationList = await _context.Locations.ToListAsync();
        return locationList.Find(l => l == location)?.LocationId;
    }

    public async Task AddLocationAsync(Location location)
    {
        await _context.Locations.AddAsync(location);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}