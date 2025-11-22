using EngageGov.Application.DTOs.Proposals;
using EngageGov.Application.Interfaces;
using EngageGov.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EngageGov.API.Controllers;

/// <summary>
/// API Controller for managing civic proposals
/// Follows RESTful conventions and API best practices
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProposalsController : ControllerBase
{
    private readonly IProposalService _proposalService;
    private readonly ILogger<ProposalsController> _logger;

    public ProposalsController(
        IProposalService proposalService,
        ILogger<ProposalsController> logger)
    {
        _proposalService = proposalService ?? throw new ArgumentNullException(nameof(proposalService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all proposals
    /// </summary>
    /// <returns>List of all proposals</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProposalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all proposals");
        
        var result = await _proposalService.GetAllProposalsAsync(cancellationToken);
        
        if (!result.IsSuccess)
        {
            _logger.LogError("Failed to get proposals: {Error}", result.ErrorMessage);
            return StatusCode(500, new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get a specific proposal by ID
    /// </summary>
    /// <param name="id">Proposal ID</param>
    /// <returns>Proposal details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProposalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting proposal {ProposalId}", id);
        
        var result = await _proposalService.GetProposalByIdAsync(id, cancellationToken);
        
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Proposal {ProposalId} not found", id);
            return NotFound(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get proposals by status
    /// </summary>
    /// <param name="status">Proposal status</param>
    /// <returns>List of proposals with the specified status</returns>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<ProposalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByStatus(ProposalStatus status, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting proposals with status {Status}", status);
        
        var result = await _proposalService.GetProposalsByStatusAsync(status, cancellationToken);
        
        if (!result.IsSuccess)
        {
            _logger.LogError("Failed to get proposals by status: {Error}", result.ErrorMessage);
            return StatusCode(500, new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get proposals by citizen ID
    /// </summary>
    /// <param name="citizenId">Citizen ID</param>
    /// <returns>List of proposals created by the citizen</returns>
    [HttpGet("citizen/{citizenId}")]
    [ProducesResponseType(typeof(IEnumerable<ProposalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByCitizen(Guid citizenId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting proposals for citizen {CitizenId}", citizenId);
        
        var result = await _proposalService.GetProposalsByCitizenAsync(citizenId, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return NotFound(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Search proposals
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <returns>List of matching proposals</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<ProposalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Search([FromQuery] string searchTerm, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest(new { error = "Search term is required" });
        }

        _logger.LogInformation("Searching proposals with term: {SearchTerm}", searchTerm);
        
        var result = await _proposalService.SearchProposalsAsync(searchTerm, cancellationToken);
        
        if (!result.IsSuccess)
        {
            _logger.LogError("Search failed: {Error}", result.ErrorMessage);
            return StatusCode(500, new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new proposal
    /// </summary>
    /// <param name="dto">Proposal creation data</param>
    /// <param name="citizenId">ID of the citizen creating the proposal</param>
    /// <returns>Created proposal</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProposalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProposalDto dto, 
        [FromQuery] Guid citizenId,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Creating new proposal for citizen {CitizenId}", citizenId);
        
        var result = await _proposalService.CreateProposalAsync(dto, citizenId, cancellationToken);
        
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to create proposal: {Error}", result.ErrorMessage);
            return BadRequest(new { error = result.ErrorMessage });
        }

        return CreatedAtAction(
            nameof(GetById), 
            new { id = result.Data!.Id }, 
            result.Data);
    }

    /// <summary>
    /// Update an existing proposal
    /// </summary>
    /// <param name="id">Proposal ID</param>
    /// <param name="dto">Proposal update data</param>
    /// <returns>Updated proposal</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProposalDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(
        Guid id, 
        [FromBody] UpdateProposalDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Updating proposal {ProposalId}", id);
        
        var result = await _proposalService.UpdateProposalAsync(id, dto, cancellationToken);
        
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to update proposal {ProposalId}: {Error}", id, result.ErrorMessage);
            return NotFound(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Delete a proposal
    /// </summary>
    /// <param name="id">Proposal ID</param>
    /// <returns>No content on success</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting proposal {ProposalId}", id);
        
        var result = await _proposalService.DeleteProposalAsync(id, cancellationToken);
        
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to delete proposal {ProposalId}: {Error}", id, result.ErrorMessage);
            return NotFound(new { error = result.ErrorMessage });
        }

        return NoContent();
    }
}
