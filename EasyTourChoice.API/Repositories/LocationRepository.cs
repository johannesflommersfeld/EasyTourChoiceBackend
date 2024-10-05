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

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}