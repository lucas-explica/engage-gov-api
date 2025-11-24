using Microsoft.AspNetCore.Mvc;
using EngageGov.Application.DTOs.Letters;
using EngageGov.Application.Interfaces;

namespace EngageGov.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LettersController : ControllerBase
{
    private readonly ILogger<LettersController> _logger;
    private readonly ILetterService _letterService;

    public LettersController(ILogger<LettersController> logger, ILetterService letterService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _letterService = letterService ?? throw new ArgumentNullException(nameof(letterService));
    }

    // GET: api/letters
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var letters = await _letterService.GetAllAsync(cancellationToken);
        return Ok(letters);
    }

    // GET: api/letters/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var letter = await _letterService.GetByIdAsync(id, cancellationToken);
        if (letter == null) return NotFound();
        return Ok(letter);
    }

    // POST: api/letters
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LetterDto dto, CancellationToken cancellationToken)
    {
        var created = await _letterService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PATCH: api/letters/{id}
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] LetterDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id) return BadRequest();
        await _letterService.UpdateAsync(dto, cancellationToken);
        return NoContent();
    }

    // DELETE: api/letters/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _letterService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
