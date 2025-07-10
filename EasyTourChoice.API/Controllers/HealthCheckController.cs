using Microsoft.AspNetCore.Mvc;

namespace EasyTourChoice.API.Controllers;

[ApiController]
[Route("api/healthcheck")]
public class HealthCheckController: ControllerBase{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult GetAllAreas()
    {
        return Ok();
    }
}