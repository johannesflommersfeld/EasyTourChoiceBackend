using Microsoft.AspNetCore.Mvc;
using EasyTourChoice.API.Services;
using EasyTourChoice.API.Models;
using AutoMapper;

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

    [HttpGet("areas/{areaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<AreaDto>>> GetArea(int areaId)
    {
        _logger.LogInformation("Specific tour data was requested");
        var area = await _areaRepository.GetAreaByIdAsync(areaId);

        if (area is null)
            return NotFound();

        return Ok(_mapper.Map<AreaDto>(area));
    }

    // TODO: add create and patch functionality
}