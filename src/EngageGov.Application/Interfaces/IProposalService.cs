using EngageGov.Application.DTOs.Common;
using EngageGov.Application.DTOs.Proposals;
using EngageGov.Domain.Enums;

namespace EngageGov.Application.Interfaces;

/// <summary>
/// Service interface for proposal operations
/// Follows Interface Segregation Principle
/// </summary>
public interface IProposalService
{
    Task<Result<ProposalDto>> CreateProposalAsync(CreateProposalDto dto, Guid citizenId, CancellationToken cancellationToken = default);
    Task<Result<ProposalDto>> GetProposalByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ProposalDto>>> GetAllProposalsAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ProposalDto>>> GetProposalsByStatusAsync(ProposalStatus status, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ProposalDto>>> GetProposalsByCitizenAsync(Guid citizenId, CancellationToken cancellationToken = default);
    Task<Result<ProposalDto>> UpdateProposalAsync(Guid id, UpdateProposalDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteProposalAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<ProposalDto>>> SearchProposalsAsync(string searchTerm, CancellationToken cancellationToken = default);
}
