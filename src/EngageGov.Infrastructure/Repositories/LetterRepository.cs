using EngageGov.Application.Interfaces;
using EngageGov.Domain.Entities;
using EngageGov.Infrastructure.Data;

namespace EngageGov.Infrastructure.Repositories;

public class LetterRepository : Repository<Letter>, ILetterRepository
{
    private readonly EngageGovDbContext _context;
    public LetterRepository(EngageGovDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
