using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using EasyTourChoice.API.Application.Models;
using EasyTourChoice.API.Controllers.Interfaces;

namespace EasyTourChoice.API.Controllers;

[ApiController]
[Route("api/areas")]
public class AreaController(
    ILogger<AreaController> logger,
    IAreaHandler areaHandler,
    IMapper mapper
    ) : ControllerBase
{
    private readonly ILogger<AreaController> _logger = logger;
    private readonly IAreaHandler _areaHandler = areaHandler;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AreaDto>>> GetAllAreas()
    {
        _logger.LogInformation("All tour data was requested");
        return Ok(await _areaHandler.GetAllAreasAsync());
    }

    [HttpGet("{areaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AreaDto>>> GetArea(int areaId)
    {
        _logger.LogInformation("Specific tour data was requested");
        var area = await _areaHandler.GetAreaByIdAsync(areaId);

        if (area is null)
            return NotFound();

        return Ok(area);
    }

    [HttpPatch("{areaId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateArea(int areaID,
           JsonPatchDocument<AreaForUpdateDto> patchDocument)
    {
        if (!await _areaHandler.AreaExistsAsync(areaID))
            return NotFound();

        var area = await _areaHandler.GetAreaByIdAsync(areaID);
        if (area == null)
            return NotFound();

        var areaToPatch = _mapper.Map<AreaForUpdateDto>(area);
        patchDocument.ApplyTo(areaToPatch, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _areaHandler.UpdateAreaAsync(areaID, areaToPatch);

        if (result.IsBadRequest)
            return BadRequest(result.ModelState);

        if (result.IsNotFound)
            return NotFound();

        return NoContent();
    }
}