using EngageGov.Application.DTOs.Citizens;
using EngageGov.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EngageGov.API.Controllers;

/// <summary>
/// API Controller for managing citizens
/// Follows RESTful conventions and API best practices
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CitizensController : ControllerBase
{
    private readonly ICitizenService _citizenService;
    private readonly ILogger<CitizensController> _logger;

    public CitizensController(
        ICitizenService citizenService,
        ILogger<CitizensController> logger)
    {
        _citizenService = citizenService ?? throw new ArgumentNullException(nameof(citizenService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all citizens
    /// </summary>
    /// <returns>List of all citizens</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CitizenDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all citizens");
        
        var result = await _citizenService.GetAllCitizensAsync(cancellationToken);
        
        if (!result.IsSuccess)
        {
            _logger.LogError("Failed to get citizens: {Error}", result.ErrorMessage);
            return StatusCode(500, new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get a specific citizen by ID
    /// </summary>
    /// <param name="id">Citizen ID</param>
    /// <returns>Citizen details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CitizenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting citizen {CitizenId}", id);
        
        var result = await _citizenService.GetCitizenByIdAsync(id, cancellationToken);
        
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Citizen {CitizenId} not found", id);
            return NotFound(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get a citizen by email
    /// </summary>
    /// <param name="email">Citizen email</param>
    /// <returns>Citizen details</returns>
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(CitizenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByEmail(string email, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting citizen by email: {Email}", email);
        
        var result = await _citizenService.GetCitizenByEmailAsync(email, cancellationToken);
        
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Citizen with email {Email} not found", email);
            return NotFound(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new citizen
    /// </summary>
    /// <param name="dto">Citizen creation data</param>
    /// <returns>Created citizen</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CitizenDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCitizenDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Creating new citizen with email {Email}", dto.Email);
        
        var result = await _citizenService.CreateCitizenAsync(dto, cancellationToken);
        
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to create citizen: {Error}", result.ErrorMessage);
            return BadRequest(new { error = result.ErrorMessage });
        }

        return CreatedAtAction(
            nameof(GetById), 
            new { id = result.Data!.Id }, 
            result.Data);
    }

    /// <summary>
    /// Delete a citizen
    /// </summary>
    /// <param name="id">Citizen ID</param>
    /// <returns>No content on success</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting citizen {CitizenId}", id);
        
        var result = await _citizenService.DeleteCitizenAsync(id, cancellationToken);
        
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to delete citizen {CitizenId}: {Error}", id, result.ErrorMessage);
            return NotFound(new { error = result.ErrorMessage });
        }

        return NoContent();
    }
}
