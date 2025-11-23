using Microsoft.AspNetCore.Mvc;
using EngageGov.Application.Interfaces;

namespace EngageGov.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RepresentativesController : ControllerBase
{
    private readonly ILogger<RepresentativesController> _logger;

    private readonly EngageGov.Application.Interfaces.IGovernmentDataService _gov;

    public RepresentativesController(ILogger<RepresentativesController> logger, EngageGov.Application.Interfaces.IGovernmentDataService gov)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gov = gov ?? throw new ArgumentNullException(nameof(gov));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string source = "camara", CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching representatives from source {Source}", source);
        var reps = await _gov.GetRepresentativesAsync(source, cancellationToken);
        return Ok(reps);
    }
}
