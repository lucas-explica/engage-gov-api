using Microsoft.AspNetCore.Mvc;

namespace EngageGov.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MetricsController : ControllerBase
{
    private readonly ILogger<MetricsController> _logger;

    public MetricsController(ILogger<MetricsController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("registrations")]
    public IActionResult Registrations()
    {
        _logger.LogInformation("Returning placeholder registrations metric");
        return Ok(new { count = 0 });
    }

    [HttpGet("top-neighborhoods")]
    public IActionResult TopNeighborhoods()
    {
        _logger.LogInformation("Returning placeholder top neighborhoods metric");
        var data = new[] { new { neighborhood = "Downtown", count = 10 } };
        return Ok(data);
    }
}
