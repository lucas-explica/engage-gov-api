using EngageGov.Domain.Entities;
using EngageGov.Domain.Enums;

namespace EngageGov.Domain.Interfaces;

/// <summary>
/// Repository interface for Proposal entity with specific query methods
/// </summary>
public interface IProposalRepository : IRepository<Proposal>
{
    Task<IEnumerable<Proposal>> GetByStatusAsync(ProposalStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Proposal>> GetByCitizenIdAsync(Guid citizenId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Proposal>> GetByTypeAsync(ProposalType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<Proposal>> GetTopVotedAsync(int count, CancellationToken cancellationToken = default);
    Task<IEnumerable<Proposal>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
