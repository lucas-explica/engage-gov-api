using EngageGov.Domain.Entities;
using EngageGov.Domain.Enums;
using EngageGov.Domain.Interfaces;
using EngageGov.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EngageGov.Infrastructure.Repositories;

/// <summary>
/// Proposal repository implementation with specific query methods
/// </summary>
public class ProposalRepository : Repository<Proposal>, IProposalRepository
{
    public ProposalRepository(EngageGovDbContext context) : base(context)
    {
    }

    public override async Task<Proposal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Citizen)
            .Include(p => p.Votes)
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Proposal>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Citizen)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Proposal>> GetByStatusAsync(
        ProposalStatus status, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Citizen)
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Proposal>> GetByCitizenIdAsync(
        Guid citizenId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Citizen)
            .Where(p => p.CitizenId == citizenId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Proposal>> GetByTypeAsync(
        ProposalType type, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Citizen)
            .Where(p => p.Type == type)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Proposal>> GetTopVotedAsync(
        int count, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Citizen)
            .OrderByDescending(p => p.VoteCount)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Proposal>> SearchAsync(
        string searchTerm, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync(cancellationToken);

        var lowerSearchTerm = searchTerm.ToLower();

        return await _dbSet
            .Include(p => p.Citizen)
            .Where(p => p.Title.ToLower().Contains(lowerSearchTerm) ||
                       p.Description.ToLower().Contains(lowerSearchTerm) ||
                       p.Location.ToLower().Contains(lowerSearchTerm))
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
