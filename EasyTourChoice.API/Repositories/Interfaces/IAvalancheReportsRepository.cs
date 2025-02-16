using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories.Interfaces;

public interface IAvalancheReportsRepository
{
    Task<IEnumerable<AvalancheReport>> GetAllReports();
    Task<AvalancheReport?> GetReportByRegionID(string regionId);
    Task SaveReport(string id, AvalancheReport report);
}