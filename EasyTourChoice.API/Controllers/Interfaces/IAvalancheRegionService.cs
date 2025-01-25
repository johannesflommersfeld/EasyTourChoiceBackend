using EasyTourChoice.API.Application.Models.BaseModels;

namespace EasyTourChoice.API.Controllers.Interfaces;

public interface IAvalancheRegionService
{
    Task<string?> GetRegionIDAsync(LocationBase location);
    public string GetRegionName(string id);
}