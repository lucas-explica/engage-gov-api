using Microsoft.AspNetCore.Mvc;

namespace EngageGov.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LettersController : ControllerBase
{
    private readonly ILogger<LettersController> _logger;

    public LettersController(ILogger<LettersController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: api/letters
    [HttpGet]
    public IActionResult GetAll(CancellationToken cancellationToken)
    {
        // Return an empty list placeholder. Frontend should handle empty arrays.
        _logger.LogInformation("Returning placeholder letters list");
        var letters = new[] {
            new { id = Guid.NewGuid(), title = "Welcome letter", status = "draft", createdAt = DateTime.UtcNow }
        };
        return Ok(letters);
    }

    // PATCH: api/letters/{id}
    [HttpPatch("{id}")]
    public IActionResult Update(Guid id, [FromBody] object body, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating letter {LetterId}", id);
        // In-memory placeholder: echo back updated resource
        var updated = new { id, updated = true };
        return Ok(updated);
    }
}
