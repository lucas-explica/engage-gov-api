using Microsoft.AspNetCore.Mvc;

namespace EngageGov.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AIController : ControllerBase
{
    private readonly ILogger<AIController> _logger;

    public AIController(ILogger<AIController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("query")]
    public IActionResult Query([FromBody] object payload)
    {
        _logger.LogInformation("AI query received");
        return Ok(new { answer = "This is a placeholder AI response." });
    }
}
