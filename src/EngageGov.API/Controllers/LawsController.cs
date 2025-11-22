using EngageGov.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EngageGov.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LawsController : ControllerBase
{
    private readonly IExternalGovService _external;
    private readonly ILogger<LawsController> _logger;

    public LawsController(IExternalGovService external, ILogger<LawsController> logger)
    {
        _external = external ?? throw new ArgumentNullException(nameof(external));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int year = 2024, [FromQuery] int items = 50, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching laws for year {Year}", year);
        var list = await _external.GetProposalsAsync(year, items, cancellationToken);
        return Ok(list);
    }
}
