using AutoMapper;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories.Interfaces;

namespace EasyTourChoice.API.Application.DataHandling;

public class TourDataHandler(
    ITourDataRepository tourDataRepository,
    ILocationRepository locationRepository,
    IMapper mapper,
    ILogger<TourDataHandler> logger,
    IAvalancheRegionService regionService,
    IAvalancheReportService reportService,
    IWeatherForecastService weatherForecastService
) : ITourDataHandler
{

    private readonly ITourDataRepository _tourDataRepository = tourDataRepository;
    private readonly ILocationRepository _locationRepository = locationRepository;
    private readonly IAvalancheRegionService _regionService = regionService;
    private readonly IAvalancheReportService _reportService = reportService;
    private readonly IWeatherForecastService _weatherForecastService = weatherForecastService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<TourDataHandler> _logger = logger;
    public async Task<List<TourDataDto>> GetAllToursAsync(ITravelPlanningService travelService)
    {
        var tourData = await _tourDataRepository.GetAllToursAsync();
        // TODO: only include travel time and distance
        return _mapper.Map<List<TourDataDto>>(tourData);
    }

    public async Task<List<TourDataDto>> GetAllToursByActvityAsync(Activity activity)
    {
        var tourData = await _tourDataRepository.GetToursByActivityAsync(activity);
        // TODO: only include travel time and distance
        return _mapper.Map<List<TourDataDto>>(tourData);
    }

    public async Task<List<TourDataDto>> GetAllToursByAreaAsync(int areaID)
    {
        var tourData = await _tourDataRepository.GetToursByAreaAsync(areaID);
        // TODO: only include travel time and distance
        return _mapper.Map<List<TourDataDto>>(tourData);
    }

    public async Task<TourDataDto?> GetPlainTourByIDAsync(int tourID)
    {
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourID);
        return _mapper.Map<TourDataDto>(tourData);
    }

    public async Task<TourDataResult> GetTourByIDAsync(int tourID)
    {
        var result = new TourDataResult();
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourID);

        if (tourData is null)
        {
            result.IsNotFound = true;
            return result;
        }

        result.TourData = _mapper.Map<TourDataDto>(tourData);
        result.IsSuccess = true;

        return result;
    }

    public async Task<WeatherForecastResult> GetWeatherForecast(int tourID)
    {
        var result = new WeatherForecastResult();
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourID);
        if (tourData is null)
        {
            result.IsNotFound = true;
            return result;
        }

        var targetLocation = await _locationRepository.GetLocationAsync(tourData.StartingLocationId);
        if (targetLocation is null)
        {
            result.IsNotFound = true;
            return result;
        }

        result.WeatherForecast = await _weatherForecastService.GetWeatherForecastAsync(targetLocation);

        result.IsSuccess = true;
        return result;
    }

    public async Task<TravelInfoResult> GetTravelInfoAsync(int tourID, Location userLocation, ITravelPlanningService travelService)
    {
        var result = new TravelInfoResult();
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourID);
        if (tourData is null)
        {
            result.IsNotFound = true;
            return result;
        }

        var targetLocation = await _locationRepository.GetLocationAsync(tourData.StartingLocationId);
        if (targetLocation is null)
        {
            result.IsNotFound = true;
            return result;
        }

        result.TravelInformation = await travelService.GetLongTravelInfoAsync(userLocation, targetLocation);
        result.IsSuccess = true;
        return result;
    }

    public async Task<BulletinResult> GetBulletinAsync(int tourID)
    {
        var result = new BulletinResult();
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourID);
        if (tourData is null)
        {
            result.IsNotFound = true;
            return result;
        }

        var targetLocation = await _locationRepository.GetLocationAsync(tourData.StartingLocationId);
        if (targetLocation is null)
        {
            result.IsNotFound = true;
            return result;
        }

        var regionID = tourData.AvalancheRegionID ?? await _regionService.GetRegionIDAsync(targetLocation);
        if (regionID is null)
        {
            result.IsNotFound = true;
            return result;
        }
        var bulletin = await _reportService.GetAvalancheReportAsync(regionID);
        if (bulletin is null)
        {
            result.IsNotFound = true;
            return result;
        }
        bulletin.RegionName = _regionService.GetRegionName(regionID);
        result.Bulletin = bulletin;
        result.IsSuccess = true;
        return result;
    }

    public async Task<bool> TourExistsAsync(int tourID)
    {
        return await _tourDataRepository.TourDataExistsAsync(tourID);
    }

    public async Task<TourDataResult> UpdateTourAsync(int tourID, TourDataForUpdateDto tourToPatch)
    {
        var result = new TourDataResult();

        if (string.IsNullOrEmpty(tourToPatch.Name))
        {
            result.IsBadRequest = true;
            result.ModelState.AddModelError("Name", "Name is required.");
            return result;
        }

        var tour = await _tourDataRepository.GetTourByIdAsync(tourID);
        if (tour == null)
        {
            result.IsNotFound = true;
            return result;
        }

        _mapper.Map(tourToPatch, tour);
        await _tourDataRepository.SaveChangesAsync();

        result.IsSuccess = true;
        var msg = $"Tour {tourID} was updated.";
        _logger.LogInformation("{msg}", msg);

        return result;
    }

    public async Task CreateTourAsync(TourData tour)
    {
        await _tourDataRepository.AddTourAsync(tour);
        await _tourDataRepository.SaveChangesAsync();
    }
}