using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories.Interfaces;

public interface IAreaRepository
{
    Task<IEnumerable<Area>> GetAllAreasAsync();
    Task<Area?> GetAreaByIdAsync(int id);
    Task<bool> AreaExistsAsync(int id);
    Task<bool> SaveChangesAsync();
}

