using EngageGov.Domain.Entities;
using EngageGov.Domain.Interfaces;
using EngageGov.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EngageGov.Infrastructure.Repositories;

/// <summary>
/// Citizen repository implementation with specific query methods
/// </summary>
public class CitizenRepository : Repository<Citizen>, ICitizenRepository
{
    public CitizenRepository(EngageGovDbContext context) : base(context)
    {
    }

    public override async Task<Citizen?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Citizen?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email == email.ToLower(), cancellationToken);
    }

    // Removed: GetByDocumentNumberAsync (property no longer exists)
    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(c => c.Email == email.ToLower(), cancellationToken);
    }

    // Removed: DocumentNumberExistsAsync (property no longer exists)
}
