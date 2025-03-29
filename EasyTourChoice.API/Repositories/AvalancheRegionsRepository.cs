using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyTourChoice.API.Repositories;
public class AvalancheRegionsRepository(TourDataContext context) : IAvalancheRegionsRepository
{
    private readonly TourDataContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<List<AvalancheRegion>> GetAllRegionsAsync()
    {
        return await _context.AvalancheRegions.ToListAsync();
    }

    public async Task<AvalancheRegion?> GetRegionByIdAsync(string id)
    {
        return await _context.AvalancheRegions.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task SaveRegionAsync(AvalancheRegion region)
    {
        var existingRegion = await _context.AvalancheRegions.FirstOrDefaultAsync(r => r.Id == region.Id);

        if (existingRegion is null)
        {
            await _context.AvalancheRegions.AddAsync(region);
        }
        else
        {
            existingRegion.Polygons = region.Polygons;
            existingRegion.Type = region.Type;
        }

        await _context.SaveChangesAsync();
    }
}
