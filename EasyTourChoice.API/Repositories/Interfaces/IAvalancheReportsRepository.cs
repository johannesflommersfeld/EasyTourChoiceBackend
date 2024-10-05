using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Repositories.Interfaces;

public interface IAvalancheReportsRepository
{
    IEnumerable<AvalancheReport> GetAllReports();
    AvalancheReport? GetReportByRegionID(string id);
    void SaveReport(string ID, AvalancheReport report);
}