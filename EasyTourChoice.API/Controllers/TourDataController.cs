using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Domain;

namespace EasyTourChoice.API.Controllers;

[ApiController]
[Route("api/tourData")]
public class TourDataController(
    ILogger<TourDataController> logger,
    IMapper mapper,
    ITourDataHandler tourDataHandler
    ) : ControllerBase
{
    private readonly ILogger<TourDataController> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly ITourDataHandler _tourDataHandler = tourDataHandler;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TourDataDto>>> GetAllTourData(
        [FromKeyedServices("OSRM")] ITravelPlanningService travelService)
    {
        var tours = await _tourDataHandler.GetAllToursAsync(travelService);
        return Ok(tours);
    }

    [HttpGet("activities/{activity}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TourDataDto>>> GetAllTourDataByActivity(Activity activity)
    {
        var tours = await _tourDataHandler.GetAllToursByActvityAsync(activity);
        return Ok(tours);
    }

    [HttpGet("areas/{areaID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TourDataDto>>> GetAllTourDataByArea(int areaID)
    {
        var tours = await _tourDataHandler.GetAllToursByAreaAsync(areaID);
        return Ok(tours);
    }

    [HttpGet("tours/{tourID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TourDataDto>> GetTourData(int tourID,
        [FromKeyedServices("OSRM")] ITravelPlanningService travelService, double? userLatitude, double? userLongitude)
    {
        Location? userLocation = null;
        if (userLatitude is not null && userLongitude is not null)
        {
            userLocation = new Location()
            {
                Latitude = (double)userLatitude,
                Longitude = (double)userLongitude,
            };
        }

        var response = await _tourDataHandler.GetTourByIDAsync(tourID, userLocation, travelService);

        if (response.IsNotFound || response.TourData is null)
            return NotFound();

        return Ok(response.TourData);
    }

    [HttpGet("tours/{tourID}/traffic")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<TourDataDto>>> GetTourDataWithTraffic(int tourID,
        [FromKeyedServices("TomTom")] ITravelPlanningService travelService, double? userLatitude, double? userLongitude)
    {
        Location userLocation = new();
        if (userLatitude is not null && userLongitude is not null)
        {
            userLocation.Latitude = (double)userLatitude;
            userLocation.Longitude = (double)userLongitude;
        }
        var response = await _tourDataHandler.GetTourByIDAsync(tourID, userLocation, travelService);

        if (response.IsNotFound || response.TourData is null)
            return NotFound();

        return Ok(response.TourData);
    }

    [HttpPost]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TourDataDto>> CreateTourData(TourDataForCreationDto tour)
    {
        var tourData = _mapper.Map<TourData>(tour);

        if (!TryValidateModel(tourData))
            return BadRequest(ModelState);

        await _tourDataHandler.CreateTourAsync(tourData);

        var tourDataForResponse = _mapper.Map<TourDataDto>(tourData);

        string msg = string.Format("New tour with id {0} was added", tourData.Id);
        _logger.LogInformation("{msg}", msg);
        return Created("GetTourData", tourDataForResponse);
    }

    [HttpPatch("{tourID}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateTourData(int tourID,
           JsonPatchDocument<TourDataForUpdateDto> patchDocument)
    {
        if (!await _tourDataHandler.TourExistsAsync(tourID))
            return NotFound();

        var tour = await _tourDataHandler.GetPlainTourByIDAsync(tourID);
        if (tour == null)
            return NotFound();

        var tourToPatch = _mapper.Map<TourDataForUpdateDto>(tour);
        patchDocument.ApplyTo(tourToPatch, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _tourDataHandler.UpdateTourAsync(tourID, tourToPatch);
        string msg = string.Format("Tour {0} was updated", tourID);
        _logger.LogInformation("{msg}", msg);

        if (result.IsBadRequest)
            return BadRequest(result.ModelState);

        if (result.IsNotFound)
            return NotFound();

        return NoContent();
    }

    // TODO: add delete functionality. Areas and locations should automatically be deleted 
    // if they are no longer referenced by any tour
}