using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories.Interfaces;

public interface IAvalancheRegionsRepository
{
    List<AvalancheRegion> GetAllRegions();
    AvalancheRegion? GetRegionById(string id);
    void SaveRegion(AvalancheRegion region);
}