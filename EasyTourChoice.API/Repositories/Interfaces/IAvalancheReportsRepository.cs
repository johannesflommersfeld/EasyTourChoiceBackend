using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories.Interfaces;

public interface IAvalancheReportsRepository
{
    Task<IEnumerable<AvalancheReport>> GetAllReportsAsync();
    Task<AvalancheReport?> GetReportByRegionIdAsync(string regionId);
    Task SaveReportAsync(AvalancheReport report);
}