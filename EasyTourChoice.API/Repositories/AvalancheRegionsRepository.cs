using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;

namespace EasyTourChoice.API.Repositories;
public class AvalancheRegionsRepository : IAvalancheRegionsRepository
{
    private readonly List<AvalancheRegion> _regions = [];

    public IEnumerable<AvalancheRegion> GetAllRegions()
    {
        return _regions;
    }

    public AvalancheRegion? GetRegionById(string ID)
    {
        return _regions.First(r => r.Id == ID);
    }

    public void SaveRegion(AvalancheRegion region)
    {
        _regions.Add(region);
    }
}
