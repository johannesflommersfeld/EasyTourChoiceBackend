using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories.Interfaces;

public interface IAvalancheRegionsRepository
{
    Task<List<AvalancheRegion>> GetAllRegionsAsync();
    Task<AvalancheRegion?> GetRegionByIdAsync(string id);
    Task SaveRegionAsync(AvalancheRegion region);
}