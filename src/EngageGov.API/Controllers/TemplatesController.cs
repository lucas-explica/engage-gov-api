using Microsoft.AspNetCore.Mvc;

namespace EngageGov.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TemplatesController : ControllerBase
{
    private readonly ILogger<TemplatesController> _logger;

    public TemplatesController(ILogger<TemplatesController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        _logger.LogInformation("Returning placeholder templates list");
        var templates = new[] {
            new { id = Guid.NewGuid(), name = "Default", subject = "Hello", body = "Hi {{name}}" }
        };
        return Ok(templates);
    }

    [HttpPost]
    public IActionResult Create([FromBody] object dto)
    {
        _logger.LogInformation("Creating template");
        var created = new { id = Guid.NewGuid() };
        return CreatedAtAction(nameof(GetAll), created);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] object dto)
    {
        _logger.LogInformation("Updating template {TemplateId}", id);
        return Ok(new { id });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _logger.LogInformation("Deleting template {TemplateId}", id);
        return NoContent();
    }

    [HttpPost("{id}/send")]
    public IActionResult Send(Guid id, [FromBody] object payload)
    {
        _logger.LogInformation("Sending template {TemplateId}", id);
        return Ok(new { sent = true, templateId = id });
    }
}
