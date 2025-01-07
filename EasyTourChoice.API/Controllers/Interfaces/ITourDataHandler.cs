using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EasyTourChoice.API.Controllers.Interfaces;

public interface ITourDataHandler
{
    Task CreateTourAsync(TourData tour);
    Task<TourDataResult> UpdateTourAsync(int tourID, TourDataForUpdateDto tourToPatch);
    Task<TourDataDto?> GetPlainTourByIDAsync(int tourID);
    Task<TourDataResult> GetTourByIDAsync(int tourID, Location? userLocation, ITravelPlanningService travelService);
    Task<bool> TourExistsAsync(int tourID);
    // TODO: allow for filtering and sorting
    Task<List<TourDataDto>> GetAllToursAsync(ITravelPlanningService travelService);
    Task<List<TourDataDto>> GetAllToursByActvityAsync(Activity activity);
    Task<List<TourDataDto>> GetAllToursByAreaAsync(int areaID);
}

public record TourDataResult
{
    public bool IsSuccess { get; set; }
    public bool IsNotFound { get; set; }
    public bool IsBadRequest { get; set; }
    public ModelStateDictionary ModelState { get; set; } = [];
    public TourDataDto? TourData { get; set; }
}