using EngageGov.Domain.Interfaces;
using EngageGov.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EngageGov.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation
/// Manages transactions and coordinates repository operations
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly EngageGovDbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    public IProposalRepository Proposals { get; }
    public ICitizenRepository Citizens { get; }

    public UnitOfWork(EngageGovDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Proposals = new ProposalRepository(_context);
        Citizens = new CitizenRepository(_context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
        _disposed = true;
    }
}
