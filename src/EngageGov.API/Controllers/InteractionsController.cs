using Microsoft.AspNetCore.Mvc;

namespace EngageGov.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class InteractionsController : ControllerBase
{
    private readonly ILogger<InteractionsController> _logger;

    public InteractionsController(ILogger<InteractionsController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        _logger.LogInformation("Returning placeholder interactions list");
        var items = new[] {
            new { id = Guid.NewGuid(), type = "like", createdAt = DateTime.UtcNow }
        };
        return Ok(items);
    }

    [HttpPost]
    public IActionResult Create([FromBody] object dto)
    {
        _logger.LogInformation("Creating interaction");
        return Created(string.Empty, new { id = Guid.NewGuid() });
    }
}
