namespace EngageGov.Domain.Interfaces;

/// <summary>
/// Unit of Work interface for managing transactions
/// Follows Unit of Work pattern
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IProposalRepository Proposals { get; }
    ICitizenRepository Citizens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
