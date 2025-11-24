using Microsoft.AspNetCore.Mvc;
using EngageGov.Application.DTOs.Templates;
using EngageGov.Application.Interfaces;

namespace EngageGov.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TemplatesController : ControllerBase
{
    private readonly ILogger<TemplatesController> _logger;
    private readonly ITemplateService _templateService;

    public TemplatesController(ILogger<TemplatesController> logger, ITemplateService templateService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
    }

    // GET: api/templates
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var templates = await _templateService.GetAllAsync(cancellationToken);
        return Ok(templates);
    }

    // GET: api/templates/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var template = await _templateService.GetByIdAsync(id, cancellationToken);
        if (template == null) return NotFound();
        return Ok(template);
    }

    // POST: api/templates
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TemplateDto dto, CancellationToken cancellationToken)
    {
        var created = await _templateService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PATCH: api/templates/{id}
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] TemplateDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id) return BadRequest();
        await _templateService.UpdateAsync(dto, cancellationToken);
        return NoContent();
    }

    // DELETE: api/templates/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _templateService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
