using EasyTourChoice.API.Application.Models;

namespace EasyTourChoice.API.Controllers.Interfaces;

public interface IAvalancheReportService
{
    Task<AvalancheReportDto?> GetValidAvalancheReportAsync(string regionID);
}