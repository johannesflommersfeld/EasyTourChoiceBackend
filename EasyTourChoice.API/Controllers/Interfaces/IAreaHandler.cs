using EasyTourChoice.API.Application.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EasyTourChoice.API.Controllers.Interfaces;

public interface IAreaHandler
{
    Task<UpdateAreaResult> UpdateAreaAsync(int areaID, AreaForUpdateDto areaToPatch);
    Task<AreaDto?> GetAreaByIdAsync(int areaID);
    Task<bool> AreaExistsAsync(int areaID);
    // TODO: allow for filtering and sorting
    Task<List<AreaDto>> GetAllAreasAsync();
}

public record UpdateAreaResult
{
    public bool IsSuccess { get; set; }
    public bool IsNotFound { get; set; }
    public bool IsBadRequest { get; set; }
    public ModelStateDictionary ModelState { get; set; } = [];
}