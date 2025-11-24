using EngageGov.Domain.Interfaces;
using EngageGov.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EngageGov.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation
/// Follows Repository pattern with Entity Framework Core
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly EngageGovDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(EngageGovDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await ExecuteWithRetry(async () => await _dbSet.FindAsync(new object[] { id }, cancellationToken).AsTask());
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteWithRetry(async () => await _dbSet.ToListAsync(cancellationToken));
    }

    // Small, generic retry wrapper to mitigate transient DB/network issues (e.g. Npgsql timeouts)
    private static async Task<TResult> ExecuteWithRetry<TResult>(Func<Task<TResult>> operation)
    {
        const int maxAttempts = 3;
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex) when (IsTransient(ex))
            {
                if (attempt >= maxAttempts) throw;
                // simple backoff
                await Task.Delay(200 * attempt);
            }
        }
    }

    private static bool IsTransient(Exception ex)
    {
        if (ex is OperationCanceledException) return true;
        if (ex is TimeoutException) return true;
        // Inspect messages for known transient indications (defensive)
        if (ex.Message != null && ex.Message.IndexOf("Timeout", StringComparison.OrdinalIgnoreCase) >= 0) return true;
        if (ex.InnerException != null && ex.InnerException.Message != null && ex.InnerException.Message.IndexOf("Timeout", StringComparison.OrdinalIgnoreCase) >= 0) return true;
        return false;
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        return entity != null;
    }
}
