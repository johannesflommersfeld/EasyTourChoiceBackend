using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using EasyTourChoice.API.Controllers.Interfaces;
using EasyTourChoice.API.Application.Models;

namespace EasyTourChoice.API.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationController(
    ILocationHandler locationHandler,
    IMapper mapper
    ) : ControllerBase
{
    private readonly ILocationHandler _locationHandler = locationHandler;
    private readonly IMapper _mapper = mapper;

    [HttpGet("{locationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocation(int locationId)
    {
        var location = await _locationHandler.GetLocationByIdAsync(locationId);

        if (location is null)
            return NotFound();

        return Ok(location);
    }

    [HttpPatch("{locationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateLocation(int locationID,
           JsonPatchDocument<LocationForUpdateDto> patchDocument)
    {
        if (!await _locationHandler.LocationExistsAsync(locationID))
            return NotFound();

        var location = await _locationHandler.GetLocationByIdAsync(locationID);
        if (location == null)
            return NotFound();

        var locationToPatch = _mapper.Map<LocationForUpdateDto>(location);
        patchDocument.ApplyTo(locationToPatch, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _locationHandler.UpdateLocationAsync(locationID, locationToPatch);

        if (result.IsBadRequest)
            return BadRequest(result.ModelState);

        if (result.IsNotFound)
            return NotFound();

        return NoContent();
    }
}