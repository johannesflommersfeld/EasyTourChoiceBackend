using AutoMapper;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Domain;
using EasyTourChoice.API.Repositories;
using EasyTourChoice.API.Repositories.Interfaces;

namespace EasyTourChoice.API.Application.DataHandling;

public class TourDataHandler(
    ITourDataRepository tourDataRepository,
    ILocationRepository locationRepository,
    IAreaRepository areaRepository,
    IMapper mapper,
    ILogger<TourDataHandler> logger,
    IAvalancheRegionService regionService,
    IAvalancheReportService reportService,
    IWeatherForecastService weatherForecastService
) : ITourDataHandler
{

    private readonly ITourDataRepository _tourDataRepository = tourDataRepository;
    private readonly ILocationRepository _locationRepository = locationRepository;
    private readonly IAreaRepository _areaRepository = areaRepository;
    private readonly IAvalancheRegionService _regionService = regionService;
    private readonly IAvalancheReportService _reportService = reportService;
    private readonly IWeatherForecastService _weatherForecastService = weatherForecastService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<TourDataHandler> _logger = logger;
    public async Task<List<TourDataDto>> GetAllToursAsync(ITravelPlanningService travelService)
    {
        var tourData = await _tourDataRepository.GetAllToursAsync();
        var result = _mapper.Map<List<TourDataDto>>(tourData);
        foreach (var tour in result)
        {
            if (tour.ActivityLocationId is not null)
            {
                var location = await _locationRepository.GetLocationAsync((int)tour.ActivityLocationId);
                tour.ActivityLocation = _mapper.Map<LocationDto>(location);
            }

            if (tour.StartingLocationId is not null)
            {
                var location = await _locationRepository.GetLocationAsync((int)tour.StartingLocationId);
                tour.StartingLocation = _mapper.Map<LocationDto>(location);
            }
        }

        // TODO: only include travel time and distance
        return result;
    }

    public async Task<List<TourDataDto>> GetAllToursByActvityAsync(Activity activity)
    {
        var tourData = await _tourDataRepository.GetToursByActivityAsync(activity);
        // TODO: only include travel time and distance
        return _mapper.Map<List<TourDataDto>>(tourData);
    }

    public async Task<List<TourDataDto>> GetAllToursByAreaAsync(int areaId)
    {
        var tourData = await _tourDataRepository.GetToursByAreaAsync(areaId);
        // TODO: only include travel time and distance
        return _mapper.Map<List<TourDataDto>>(tourData);
    }

    public async Task<TourDataDto?> GetPlainTourByIDAsync(int tourId)
    {
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourId);
        return _mapper.Map<TourDataDto>(tourData);
    }

    public async Task<TourDataResult> GetTourByIDAsync(int tourId)
    {
        var result = new TourDataResult();
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourId);

        if (tourData is null)
        {
            result.IsNotFound = true;
            return result;
        }

        result.TourData = _mapper.Map<TourDataDto>(tourData);

        if (tourData.ActivityLocationId is not null)
        {
            var location = await _locationRepository.GetLocationAsync((int)tourData.ActivityLocationId);
            result.TourData.ActivityLocation = _mapper.Map<LocationDto>(location);
        }

        if (tourData.StartingLocationId is not null)
        {
            var location = await _locationRepository.GetLocationAsync((int)tourData.StartingLocationId);
            result.TourData.StartingLocation = _mapper.Map<LocationDto>(location);
        }

        result.IsSuccess = true;

        return result;
    }

    public async Task<WeatherForecastResult> GetWeatherForecast(int tourId)
    {
        var result = new WeatherForecastResult();
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourId);
        if (tourData?.StartingLocationId is null)
        {
            result.IsNotFound = true;
            return result;
        }

        var targetLocation = await _locationRepository.GetLocationAsync((int)tourData.StartingLocationId);
        if (targetLocation is null)
        {
            result.IsNotFound = true;
            return result;
        }

        result.WeatherForecast = await _weatherForecastService.GetWeatherForecastAsync(targetLocation);

        result.IsSuccess = true;
        return result;
    }

    public async Task<TravelInfoResult> GetTravelInfoAsync(int tourId, Location userLocation, ITravelPlanningService travelService)
    {
        var result = new TravelInfoResult();
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourId);
        if (tourData?.StartingLocationId is null)
        {
            result.IsNotFound = true;
            return result;
        }

        var targetLocation = await _locationRepository.GetLocationAsync((int)tourData.StartingLocationId);
        if (targetLocation is null)
        {
            result.IsNotFound = true;
            return result;
        }

        result.TravelInformation = await travelService.GetLongTravelInfoAsync(userLocation, targetLocation);
        result.IsSuccess = true;
        return result;
    }

    public async Task<BulletinResult> GetBulletinAsync(int tourId)
    {
        var result = new BulletinResult();
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourId);
        if (tourData?.StartingLocationId is null)
        {
            result.IsNotFound = true;
            return result;
        }

        var targetLocation = await _locationRepository.GetLocationAsync((int)tourData.StartingLocationId);
        if (targetLocation is null)
        {
            result.IsNotFound = true;
            return result;
        }

        var regionId = tourData.AvalancheRegionId ?? await _regionService.GetRegionIDAsync(targetLocation);
        if (regionId is null)
        {
            result.IsNotFound = true;
            return result;
        }
        var bulletin = await _reportService.GetAvalancheReportAsync(regionId);
        if (bulletin is null)
        {
            result.IsNotFound = true;
            return result;
        }
        bulletin.RegionName = _regionService.GetRegionName(regionId);
        result.Bulletin = bulletin;
        result.IsSuccess = true;
        return result;
    }

    public async Task<bool> TourExistsAsync(int tourId)
    {
        return await _tourDataRepository.TourDataExistsAsync(tourId);
    }

    public async Task<TourDataResult> UpdateTourAsync(int tourId, TourDataForUpdateDto tourToPatch)
    {
        var result = new TourDataResult();

        if (string.IsNullOrEmpty(tourToPatch.Name))
        {
            result.IsBadRequest = true;
            result.ModelState.AddModelError("Name", "Name is required.");
            return result;
        }

        var tour = await _tourDataRepository.GetTourByIdAsync(tourId);
        if (tour == null)
        {
            result.IsNotFound = true;
            return result;
        }

        tour.ActivityLocation = null;
        tour.StartingLocation = null;
        await _tourDataRepository.SaveChangesAsync();

        if (tourToPatch.StartingLocation is not null)
        {
            tourToPatch.StartingLocationId = await _locationRepository.FindLocationIdAsync(_mapper.Map<Location>(tourToPatch.StartingLocation))
                                             ?? await GetNewLocationIdAsync(_mapper.Map<Location>(tourToPatch.StartingLocation));
        }
        
        if (tourToPatch.ActivityLocation is not null)
        {
            tourToPatch.ActivityLocationId = await _locationRepository.FindLocationIdAsync(_mapper.Map<Location>(tourToPatch.ActivityLocation))
                                             ?? await GetNewLocationIdAsync(_mapper.Map<Location>(tourToPatch.ActivityLocation));       
        }
        
        _mapper.Map(tourToPatch, tour);
        await _tourDataRepository.SaveChangesAsync();

        result.IsSuccess = true;
        var msg = $"Tour {tourId} was updated.";
        _logger.LogInformation("{msg}", msg);

        return result;
    }

    public async Task CreateTourAsync(TourData tour)
    {
        if (tour.StartingLocation is not null)
        {
            tour.StartingLocationId = await _locationRepository.FindLocationIdAsync(tour.StartingLocation)
                                               ?? await GetNewLocationIdAsync(tour.StartingLocation);
            tour.StartingLocation = null;
        }
        
        if (tour.ActivityLocation is not null)
        {
            tour.ActivityLocationId = await _locationRepository.FindLocationIdAsync(tour.ActivityLocation)
                ?? await GetNewLocationIdAsync(tour.ActivityLocation);
            tour.ActivityLocation = null;
        }

        if (tour.Area is not null)
        {
            tour.AreaId = await _areaRepository.FindAreaAsync(tour.Area.Name);
        }

        await _tourDataRepository.AddTourAsync(tour);
        await _tourDataRepository.SaveChangesAsync();
    }

    private async Task<int> GetNewLocationIdAsync(Location location)
    {
        if (location.LocationId is not null)
        {
            return (int)location.LocationId;
        }
        await _locationRepository.AddLocationAsync(location);
        await _locationRepository.SaveChangesAsync();
        return  await _locationRepository.FindLocationIdAsync(location) 
            ?? throw new NullReferenceException("Location could not be added to database.");
    }
}