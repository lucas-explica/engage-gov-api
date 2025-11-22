using Microsoft.AspNetCore.Mvc;
using EngageGov.Application.Interfaces;

namespace EngageGov.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RepresentativesController : ControllerBase
{
    private readonly ILogger<RepresentativesController> _logger;

    private readonly IExternalGovService _external;

    public RepresentativesController(ILogger<RepresentativesController> logger, IExternalGovService external)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _external = external ?? throw new ArgumentNullException(nameof(external));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching representatives from external API");
        var reps = await _external.GetRepresentativesAsync(cancellationToken);
        return Ok(reps);
    }
}
