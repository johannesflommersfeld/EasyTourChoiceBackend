using Microsoft.EntityFrameworkCore;
using EasyTourChoice.API.DbContexts;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;

namespace EasyTourChoice.API.Repositories;

public class TourDataRepository(TourDataContext context) : ITourDataRepository
{
    private readonly TourDataContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<IEnumerable<TourData>> GetAllToursAsync()
    {
        return await _context.Tours
            .Include(t => t.StartingLocation)
            .Include(t => t.ActivityLocation)
            .ToListAsync();
    }

    public async Task<IEnumerable<TourData>> GetToursByActivityAsync(Activity activity)
    {
        return await _context.Tours.Where(t => t.ActivityType == activity).ToListAsync();
    }

    public async Task<IEnumerable<TourData>> GetToursByAreaAsync(int areaId)
    {
        return await _context.Tours.Where(t => t.AreaId == areaId).ToListAsync();
    }

    public async Task<TourData?> GetTourByIdAsync(int id)
    {
        return await _context.Tours
            .Include(t => t.StartingLocation)
            .Include(t => t.ActivityLocation)
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task AddTourAsync(TourData tourData)
    {
        await _context.Tours.AddAsync(tourData);
    }
    
    public void DeleteTour(TourData tourData)
    {
        _context.Tours.Remove(tourData);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }

    public async Task<bool> TourDataExistsAsync(int id)
    {
        return await _context.Tours.AnyAsync(t => t.Id == id);
    }
}