using EngageGov.Domain.Entities;
using System.Threading;

namespace EngageGov.Application.Interfaces;

public interface ILetterRepository
{
    Task<IEnumerable<Letter>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Letter?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Letter> AddAsync(Letter entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Letter entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Letter entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
