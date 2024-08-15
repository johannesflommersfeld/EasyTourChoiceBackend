using Microsoft.AspNetCore.Mvc;
using EasyTourChoice.API.Services;
using EasyTourChoice.API.Models;

namespace EasyTourChoice.API.Controllers;

[ApiController]
[Route("api/tourData")]
public class TourDataController(
    ITourDataRepository tourDataRepository, 
    ILogger<TourDataController> logger
    ) : ControllerBase
{
    private readonly ILogger<TourDataController> _logger = logger;
    private readonly ITourDataRepository _tourDataRepository = tourDataRepository;

    [HttpGet]
    public ActionResult<IEnumerable<TourDataDto>> GetAllTourData()
    {
        _logger.LogInformation("All tour data was requested");
        return Ok(_tourDataRepository.GetAll());
    }

    [HttpGet("{activity}")]
    public ActionResult<IEnumerable<TourDataDto>> GetTourDataByActivity(Activity activity)
    {
        _logger.LogInformation("Activity specific tour data was requested");
        return Ok(_tourDataRepository.GetAllByActivity(activity));
    }

}