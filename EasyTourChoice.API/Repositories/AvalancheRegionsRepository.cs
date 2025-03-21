using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;

namespace EasyTourChoice.API.Repositories;
public class AvalancheRegionsRepository(TourDataContext context) : IAvalancheRegionsRepository
{
    private readonly TourDataContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public List<AvalancheRegion> GetAllRegions()
    {
        return _context.AvalancheRegions.ToList();
    }

    public AvalancheRegion? GetRegionById(string id)
    {
        return _context.AvalancheRegions.FirstOrDefault(r => r.Id == id);
    }

    public void SaveRegion(AvalancheRegion region)
    {
        var existingRegion = _context.AvalancheRegions.FirstOrDefault(r => r.Id == region.Id);

        if (existingRegion is null)
        {
            // Add new region
            _context.AvalancheRegions.Add(region);
        }
        else
        {
            _context.Entry(existingRegion).CurrentValues.SetValues(region);
        }

        _context.SaveChanges();
    }
}
