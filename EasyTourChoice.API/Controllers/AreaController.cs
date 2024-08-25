using Microsoft.AspNetCore.Mvc;
using EasyTourChoice.API.Services;
using EasyTourChoice.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace EasyTourChoice.API.Controllers;

[ApiController]
[Route("api/areas")]
public class AreaController(
    IAreaRepository areaRepository,
    ILogger<AreaController> logger,
    IMapper mapper
    ) : ControllerBase
{
    private readonly ILogger<AreaController> _logger = logger;
    private readonly IAreaRepository _areaRepository = areaRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AreaDto>>> GetAllAreas()
    {
        _logger.LogInformation("All tour data was requested");
        var areaList = (await _areaRepository.GetAllAreasAsync()).ToList();

        return Ok(_mapper.Map<IList<AreaDto>>(areaList));
    }

    [HttpGet("{areaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AreaDto>>> GetArea(int areaId)
    {
        _logger.LogInformation("Specific tour data was requested");
        var area = await _areaRepository.GetAreaByIdAsync(areaId);

        // include weather and avalanche information

        if (area is null)
            return NotFound();

        return Ok(_mapper.Map<AreaDto>(area));
    }

    [HttpPatch("{areaId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateArea(int areaID,
           JsonPatchDocument<AreaForUpdateDto> patchDocument)
    {
        if (!await _areaRepository.AreaExistsAsync(areaID))
            return NotFound();

        var area = await _areaRepository.GetAreaByIdAsync(areaID);
        if (area == null)
            return NotFound();

        var areaToPatch = _mapper.Map<AreaForUpdateDto>(area);
        patchDocument.ApplyTo(areaToPatch, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!TryValidateModel(areaToPatch))
            return BadRequest(ModelState);
        
        _mapper.Map(areaToPatch, area);
        await _areaRepository.SaveChangesAsync();

        string msg = string.Format("Area {0} was updated", areaID);
        _logger.LogInformation("{msg}", msg);
        return NoContent();
    }
}