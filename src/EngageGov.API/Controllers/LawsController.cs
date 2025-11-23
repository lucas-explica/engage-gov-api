using EngageGov.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EngageGov.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LawsController : ControllerBase
{
    private readonly EngageGov.Application.Interfaces.IGovernmentDataService _gov;
    private readonly ILogger<LawsController> _logger;

    public LawsController(EngageGov.Application.Interfaces.IGovernmentDataService gov, ILogger<LawsController> logger)
    {
        _gov = gov ?? throw new ArgumentNullException(nameof(gov));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string source = "camara", [FromQuery] int year = 2025, [FromQuery] int items = 50, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching laws for year {Year} from source {Source}", year, source);
        var list = await _gov.GetLawsAsync(source, year, items, cancellationToken);
        return Ok(list);
    }

    [HttpGet("{externalId}")]
    public async Task<IActionResult> GetByExternalId(string externalId, CancellationToken cancellationToken)
    {
        var law = await _gov.GetLawByExternalIdAsync("camara", externalId, cancellationToken);
        if (law == null)
            return NotFound();
        return Ok(law);
    }
}
