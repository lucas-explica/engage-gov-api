using EngageGov.Application.DTOs.Templates;

namespace EngageGov.Application.Interfaces;

public interface ITemplateService
{
    Task<IEnumerable<TemplateDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TemplateDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TemplateDto> CreateAsync(TemplateDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(TemplateDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
