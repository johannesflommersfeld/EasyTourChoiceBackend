using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using EasyTourChoice.API.Services;
using EasyTourChoice.API.Models;
using AutoMapper;

namespace EasyTourChoice.API.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationController(
    ILocationRepository locationRepository,
    ILogger<TourDataController> logger,
    IMapper mapper
    ) : ControllerBase
{
    private readonly ILogger<TourDataController> _logger = logger;
    private readonly ILocationRepository _locationRepository = locationRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet("{locationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocation(int locationId)
    {
        var location = await _locationRepository.GetLocationAsync(locationId);

        if (location is null)
            return NotFound();

        return Ok(_mapper.Map<LocationDto>(location));
    }

    [HttpPatch("{locationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateLocation(int locationId,
           JsonPatchDocument<LocationForUpdateDto> patchDocument)
    {
        if (!await _locationRepository.LocationExistsAsync(locationId))
            return NotFound();

        var location = await _locationRepository.GetLocationAsync(locationId);
        if (location == null)
            return NotFound();

        var locationToPatch = _mapper.Map<LocationForUpdateDto>(location);
        patchDocument.ApplyTo(locationToPatch, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!TryValidateModel(locationToPatch))
            return BadRequest(ModelState);
        
        _mapper.Map(locationToPatch, location);
        await _locationRepository.SaveChangesAsync();

        string msg = string.Format("Location {0} was updated", locationId);
        _logger.LogInformation("{msg}", msg);
        return NoContent();
    }
}