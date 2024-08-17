using Microsoft.AspNetCore.Mvc;
using EasyTourChoice.API.Services;
using EasyTourChoice.API.Models;
using AutoMapper;
using EasyTourChoice.API.Entities;

namespace EasyTourChoice.API.Controllers;

[ApiController]
[Route("api/tourData")]
public class TourDataController(
    ITourDataRepository tourDataRepository,
    ILogger<TourDataController> logger,
    IMapper mapper
    ) : ControllerBase
{
    private readonly ILogger<TourDataController> _logger = logger;
    private readonly ITourDataRepository _tourDataRepository = tourDataRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TourDataDto>>> GetAllTourData()
    {
        _logger.LogInformation("All tour data was requested");
        var tourData = await _tourDataRepository.GetAllToursAsync();
        // TODO: include the previews of weather, avalanche, and travel reports

        return Ok(_mapper.Map<IEnumerable<TourDataDto>>(tourData));
    }

    [HttpGet("activities/{activity}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TourDataDto>>> GetAllTourDataByActivity(Activity activity)
    {
        _logger.LogInformation("Activity-specific tour data was requested");
        var tourData = await _tourDataRepository.GetToursByActivityAsync(activity);
        // TODO: include the previews of weather, avalanche, and travel reports

        return Ok(_mapper.Map<IEnumerable<TourDataDto>>(tourData));
    }

    [HttpGet("areas/{areaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TourDataDto>>> GetAllTourDataByArea(int areaId)
    {
        _logger.LogInformation("Area-specific tour data was requested");
        var tourData = await _tourDataRepository.GetToursByAreaAsync(areaId);

        return Ok(_mapper.Map<IEnumerable<TourDataDto>>(tourData));
    }

    [HttpGet("tours/{tourId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<TourDataDto>>> GetTourData(int tourId, bool useTraffic)
    {
        _logger.LogInformation("Specific tour data was requested");
        var tourData = await _tourDataRepository.GetTourByIdAsync(tourId);

        if (tourData is null)
            return NotFound();

        // TODO: include the full travel, weather and avalanche details

        return Ok(_mapper.Map<TourDataDto>(tourData));
    }

    [HttpPost]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<TourDataDto> CreateTourData(TourDataForCreationDto tour)
    {
        // TODO: connect to Database
        var tourData = _mapper.Map<TourData>(tour);

        if (!TryValidateModel(tourData))
            return BadRequest(ModelState);

        var tourDataForResponse = _mapper.Map<TourDataDto>(tourData);
        return Created("GetTourData", tourDataForResponse);
    }

    // TODO: add patch functionality
}