using EngageGov.Domain.Entities;

namespace EngageGov.Application.Interfaces;

public interface ITemplateRepository
{
    Task<IEnumerable<Template>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Template?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Template> AddAsync(Template entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Template entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Template entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
