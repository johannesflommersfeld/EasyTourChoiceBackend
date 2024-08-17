using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Services;

public interface IAreaRepository
{
    Task<IEnumerable<Area>> GetAllAreasAsync();

    Task<Area?> GetAreaByIdAsync(int id);
}

