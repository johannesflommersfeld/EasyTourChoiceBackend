using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Services;

public interface ILocationRepository
{
    Task<bool> LocationExistsAsync(int id);
    Task<Location?> GetLocationAsync(int id);
    Task<bool> SaveChangesAsync();
}

