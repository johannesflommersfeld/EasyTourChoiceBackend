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
        return await _context.Tours.Where(t => t.AreaId == areaId).ToListAsync();
    }

    public async Task<TourData?> GetTourByIdAsync(int id)
    {
        return await _context.Tours.Where(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddTourAsync(TourData tourData)
    {
        if (tourData.StartingLocation != null)
        {
            var locationList = await _context.Locations.ToListAsync();
            var location = locationList.Find(l => l == tourData.StartingLocation);

            if (location != null)
            {
                tourData.StartingLocationId = location.LocationId;
                tourData.StartingLocation = null;
            }
        }

        if (tourData.ActivityLocation != null)
        {
            var locationList = await _context.Locations.ToListAsync();
            var location = locationList.Find(l => l == tourData.ActivityLocation);
            if (location != null)
            {
                tourData.ActivityLocationId = location.LocationId;
                tourData.ActivityLocation = null;
            }
        }

        if (tourData.Area != null)
        {
            var area = await _context.Areas.Where(a => a.Name == tourData.Area.Name).FirstOrDefaultAsync();
            if (area != null)
            {
                tourData.AreaId = area.AreaId;
                tourData.Area = null;
            }
        }

        _context.Tours.Add(tourData);
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