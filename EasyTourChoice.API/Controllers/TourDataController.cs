using Microsoft.AspNetCore.Mvc;
using EasyTourChoice.API.Services;
using EasyTourChoice.API.Models;
using AutoMapper;
using EasyTourChoice.API.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace EasyTourChoice.API.Controllers;

[ApiController]
[Route("api/tourData")]
public class TourDataController(
    ITourDataRepository tourDataRepository,
    ILocationRepository locationRepository,
    ILogger<TourDataController> logger,
    IMapper mapper,
    EAWSRegionService regionService,
    EAWSReportService reportService,
    IWeatherForecastService weatherForecastService
    ) : ControllerBase
{
    private readonly ILogger<TourDataController> _logger = logger;
    private readonly ITourDataRepository _tourDataRepository = tourDataRepository;
    private readonly ILocationRepository _locationRepository = locationRepository;
    private readonly IMapper _mapper = mapper;
    private readonly EAWSRegionService _regionService = regionService;
    private readonly EAWSReportService _reportService = reportService;
    private readonly IWeatherForecastService _weatherForecastService = weatherForecastService;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TourDataDto>>> GetAllTourData(
        [FromKeyedServices("OSRM")] ITravelPlanningService travelService)
    {
        var tourData = await _tourDataRepository.GetAllToursAsync();
        // TODO: only include travel time and distance

        return Ok(_mapper.Map<IEnumerable<TourDataDto>>(tourData));
    }

    [HttpGet("activities/{activity}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TourDataDto>>> GetAllTourDataByActivity(Activity activity)
    {
        var tourData = await _tourDataRepository.GetToursByActivityAsync(activity);
        // TODO: only include travel time and distance
        return Ok(_mapper.Map<IEnumerable<TourDataDto>>(tourData));
    }

    [HttpGet("areas/{areaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TourDataDto>>> GetAllTourDataByArea(int areaId)
    {
        var tourData = await _tourDataRepository.GetToursByAreaAsync(areaId);

        return Ok(_mapper.Map<IEnumerable<TourDataDto>>(tourData));
    }

    [HttpGet("tours/{tourId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TourDataDto>> GetTourData(int tourId,
        [FromKeyedServices("OSRM")] ITravelPlanningService travelService, double? userLatitude, double? userLongitude)
    {
        Location userLocation = new();
        if (userLatitude is not null && userLongitude is not null)
        {
            userLocation.Latitude = (double)userLatitude;
            userLocation.Longitude = (double)userLongitude;
        }
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourId);

        if (tourData is null)
            return NotFound();

        var tourDataDto = await GetTourData(tourData, travelService, userLocation);

        return Ok(tourDataDto);
    }

    [HttpGet("tours/{tourId}/traffic")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<TourDataDto>>> GetTourDataWithTraffic(int tourId,
        [FromKeyedServices("TomTom")] ITravelPlanningService travelService, double? userLatitude, double? userLongitude)
    {
        Location userLocation = new();
        if (userLatitude is not null && userLongitude is not null)
        {
            userLocation.Latitude = (double)userLatitude;
            userLocation.Longitude = (double)userLongitude;
        }
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourId);

        if (tourData is null)
            return NotFound();

        var tourDataDto = await GetTourData(tourData, travelService, userLocation);

        return Ok(tourDataDto);
    }

    private async Task<TourDataDto> GetTourData(TourData tourData, ITravelPlanningService travelService, Location? userLocation)
    {
        var tourDataDto = _mapper.Map<TourDataDto>(tourData);
        var targetLocation = await _locationRepository.GetLocationAsync(tourDataDto.StartingLocationId);
        if (targetLocation is null)
            return tourDataDto;

        if (userLocation is not null)
        {
            var travelInfo = await travelService.GetLongTravelInfoAsync(userLocation, targetLocation);
            tourDataDto.TravelDetails = travelInfo;
        }

        var regionID = _regionService.GetRegionID(targetLocation);
        if (regionID is not null)
        {
            tourDataDto.Bulletin = await _reportService.GetLatestAvalancheReportAsync(regionID);
        }

        tourDataDto.WeatherForecast = await _weatherForecastService.GetWeatherForecastAsync(targetLocation);
        return tourDataDto;
    }

    [HttpPost]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TourDataDto>> CreateTourDataWithoutTraffic(TourDataForCreationDto tour)
    {
        var tourData = _mapper.Map<TourData>(tour);

        if (!TryValidateModel(tourData))
            return BadRequest(ModelState);

        await _tourDataRepository.AddTourAsync(tourData);
        await _tourDataRepository.SaveChangesAsync();

        var tourDataForResponse = _mapper.Map<TourDataDto>(tourData);

        string msg = string.Format("New tour with id {0} was added", tourData.Id);
        _logger.LogInformation("{msg}", msg);
        return Created("GetTourData", tourDataForResponse);
    }

    [HttpPatch("{tourId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateTourData(int tourId,
           JsonPatchDocument<TourDataForUpdateDto> patchDocument)
    {
        if (!await _tourDataRepository.TourDataExistsAsync(tourId))
            return NotFound();

        var tour = await _tourDataRepository.GetTourByIdAsync(tourId);
        if (tour == null)
            return NotFound();

        var tourToPatch = _mapper.Map<TourDataForUpdateDto>(tour);
        patchDocument.ApplyTo(tourToPatch, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!TryValidateModel(tourToPatch))
            return BadRequest(ModelState);

        _mapper.Map(tourToPatch, tour);
        await _tourDataRepository.SaveChangesAsync();

        string msg = string.Format("Tour {0} was updated", tourId);
        _logger.LogInformation("{msg}", msg);
        return NoContent();
    }

    // TODO: add delete functionality. Areas and locations should automatically be deleted 
    // if they are no longer referenced by any tour
}