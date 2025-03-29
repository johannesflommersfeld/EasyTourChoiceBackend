using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EasyTourChoice.API.Controllers.Interfaces;

public interface ITourDataHandler
{
    Task CreateTourAsync(TourData tour);
    Task<TourDataResult> UpdateTourAsync(int tourId, TourDataForUpdateDto tourToPatch);
    Task<TourDataResult> DeleteTourAsync(int tourId);
    Task<TourDataDto?> GetPlainTourByIDAsync(int tourId);
    Task<TourDataResult> GetTourByIDAsync(int tourId);
    Task<TravelInfoResult> GetTravelInfoAsync(int tourId, Location userLocation, ITravelPlanningService travelService);
    Task<WeatherForecastResult> GetWeatherForecast(int tourId);
    Task<BulletinResult> GetBulletinAsync(int tourId);

    Task<bool> TourExistsAsync(int tourId);
    // TODO: allow for filtering and sorting
    Task<List<TourDataDto>> GetAllToursAsync(Location? userLocation, ITravelPlanningService travelService);
    Task<List<TourDataDto>> GetAllToursByActvityAsync(Activity activity);
    Task<List<TourDataDto>> GetAllToursByAreaAsync(int areaId);
}

public record TourDataResult
{
    public bool IsSuccess { get; set; }
    public bool IsNotFound { get; set; }
    public bool IsBadRequest { get; set; }
    public ModelStateDictionary ModelState { get; set; } = [];
    public TourDataDto? TourData { get; set; }
}

public record WeatherForecastResult
{
    public bool IsSuccess { get; set; }
    public bool IsNotFound { get; set; }
    public bool IsBadRequest { get; set; }
    public ModelStateDictionary ModelState { get; set; } = [];
    public WeatherForecastDto? WeatherForecast { get; set; }
}

public record TravelInfoResult
{
    public bool IsSuccess { get; set; }
    public bool IsNotFound { get; set; }
    public bool IsBadRequest { get; set; }
    public ModelStateDictionary ModelState { get; set; } = [];
    public TravelInformationWithRouteDto? TravelInformation { get; set; }
}

public record BulletinResult
{
    public bool IsSuccess { get; set; }
    public bool IsNotFound { get; set; }
    public bool IsBadRequest { get; set; }
    public ModelStateDictionary ModelState { get; set; } = [];
    public AvalancheReportDto? Bulletin { get; set; }
}