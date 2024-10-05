using Microsoft.EntityFrameworkCore;
using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Repositories.Interfaces;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories;

public class AreaRepository(TourDataContext context) : IAreaRepository
{
    private readonly TourDataContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<IEnumerable<Area>> GetAllAreasAsync()
    {
        return await _context.Areas.Include(a => a.Tours).ToListAsync();
    }

    public async Task<Area?> GetAreaByIdAsync(int id)
    {
        return await _context.Areas.Include(a => a.Tours).Where(t => t.AreaId == id).FirstOrDefaultAsync();
    }

    public async Task<bool> AreaExistsAsync(int id)
    {
        return await _context.Areas.AnyAsync(l => l.AreaId == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}