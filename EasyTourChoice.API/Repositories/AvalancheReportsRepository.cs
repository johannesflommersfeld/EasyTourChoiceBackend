using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;

namespace EasyTourChoice.API.Repositories;
public class AvalancheReportsRepository : IAvalancheReportsRepository
{
    private readonly Dictionary<string, AvalancheReport> _reports = [];

    public IEnumerable<AvalancheReport> GetAllReports()
    {
        return _reports.Values.ToList();
    }

    public AvalancheReport? GetReportByRegionID(string id)
    {
        _reports.TryGetValue(id, out AvalancheReport? report);
        return report;
    }

    public void SaveReport(string id, AvalancheReport report)
    {
        _reports[id] = report;
    }
}
