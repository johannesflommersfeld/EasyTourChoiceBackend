using EasyTourChoice.API.Application.Models;

namespace EasyTourChoice.API.Controllers.Interfaces;

public interface IAvalancheReportService
{
#if DEBUG
    Task<AvalancheReportDto?> GetAvalancheReportAsync(string regionId, bool mustBeValid=true);
#else
    Task<AvalancheReportDto?> GetAvalancheReportAsync(string regionID);
#endif
}