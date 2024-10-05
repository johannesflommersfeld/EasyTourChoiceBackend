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
        return _reports[id];
    }

    public void SaveReport(string ID, AvalancheReport report)
    {
        _reports[ID] = report;
    }
}
