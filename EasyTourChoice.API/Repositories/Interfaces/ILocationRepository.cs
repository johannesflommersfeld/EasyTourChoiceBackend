using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories.Interfaces;

public interface ILocationRepository
{
    Task<bool> LocationExistsAsync(int id);
    Task<Location?> GetLocationAsync(int id);
    Task<int?> FindLocationIdAsync(Location location);
    Task AddLocationAsync(Location location);
    Task<bool> SaveChangesAsync();
}

