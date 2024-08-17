using Microsoft.EntityFrameworkCore;
using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Services;

public class TourDataRepository(TourDataContext context) : ITourDataRepository
{
    private readonly TourDataContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<IEnumerable<TourData>> GetAllToursAsync()
    {
        return await _context.Tours.ToListAsync();
    }

    public async Task<IEnumerable<TourData>> GetToursByActivityAsync(Activity activity)
    {
        return await _context.Tours.Where(t => t.ActivityType == activity).ToListAsync();
    }

    public async Task<IEnumerable<TourData>> GetToursByAreaAsync(int areaId)
    {
        return await _context.Tours.Where(t =>  t.AreaId == areaId).ToListAsync();
    }

    public async Task<TourData?> GetTourByIdAsync(int id)
    {
        return await _context.Tours.Where(t => t.Id == id).FirstOrDefaultAsync();
    }
}