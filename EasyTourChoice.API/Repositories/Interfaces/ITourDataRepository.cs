using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories.Interfaces;

public interface ITourDataRepository
{
    Task<IEnumerable<TourData>> GetAllToursAsync();
    Task<IEnumerable<TourData>> GetToursByActivityAsync(Activity activity);
    Task<IEnumerable<TourData>> GetToursByAreaAsync(int areaId);
    Task<TourData?> GetTourByIdAsync(int id);
    Task<bool> TourDataExistsAsync(int id);
    Task AddTourAsync(TourData tourData);
    Task<bool> SaveChangesAsync();
}

