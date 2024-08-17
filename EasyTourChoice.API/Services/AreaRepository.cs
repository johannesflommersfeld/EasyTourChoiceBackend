using Microsoft.EntityFrameworkCore;
using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Services;

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
}