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
    public ActionResult<IEnumerable<TourDataDto>> GetAllTourData()
    {
        _logger.LogInformation("All tour data was requested");
        var tourData = _tourDataRepository.GetAll();

        return Ok(_mapper.Map<IEnumerable<TourDataDto>>(tourData));
    }

    [HttpGet("activities/{activity}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<TourDataDto>> GetAllTourDataByActivity(Activity activity)
    {
        _logger.LogInformation("Activity specific tour data was requested");
        var tourData = _tourDataRepository.GetAllByActivity(activity);

        return Ok(_mapper.Map<IEnumerable<TourDataDto>>(tourData));
    }

    [HttpGet("tours/{tourId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<TourDataDto>> GetTourData(int tourId)
    {
        _logger.LogInformation("Activity specific tour data was requested");
        var tourData = _tourDataRepository.GetTourData(tourId);

        return Ok(_mapper.Map<TourDataDto>(tourData));
    }

    [HttpPost]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<TourDataDto> CreateTourData(TourDataForCreationDto tour)
    {
        var tourData = _mapper.Map<TourData>(tour);

        if (!TryValidateModel(tourData))
            return BadRequest(ModelState);

        var tourDataForResponse = _mapper.Map<TourDataDto>(tourData);
        return Created("GetTourData", tourDataForResponse);
    }
}