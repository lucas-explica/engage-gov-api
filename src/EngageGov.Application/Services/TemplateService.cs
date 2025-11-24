using EngageGov.Application.DTOs.Templates;
using EngageGov.Application.Interfaces;
using EngageGov.Domain.Entities;

namespace EngageGov.Application.Services;

public class TemplateService : ITemplateService
{
    private readonly ITemplateRepository _repo;

    public TemplateService(ITemplateRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<TemplateDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _repo.GetAllAsync(cancellationToken);
        return items.Select(t => new TemplateDto(t.Id, t.Name, t.Category, t.Content, t.UsageCount, t.Variables?.ToArray() ?? new string[0]));
    }

    public async Task<TemplateDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var t = await _repo.GetByIdAsync(id, cancellationToken);
        return t == null ? null : new TemplateDto(t.Id, t.Name, t.Category, t.Content, t.UsageCount, t.Variables?.ToArray() ?? new string[0]);
    }

    public async Task<TemplateDto> CreateAsync(TemplateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new Template
        {
            Name = dto.Name,
            Category = dto.Category,
            Content = dto.Content,
            UsageCount = dto.UsageCount,
            Variables = dto.Variables?.ToList() ?? new List<string>()
        };
        await _repo.AddAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);
        return new TemplateDto(entity.Id, entity.Name, entity.Category, entity.Content, entity.UsageCount, entity.Variables?.ToArray() ?? new string[0]);
    }

    public async Task UpdateAsync(TemplateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repo.GetByIdAsync(dto.Id, cancellationToken);
        if (entity == null) return;
        entity.Name = dto.Name;
        entity.Category = dto.Category;
        entity.Content = dto.Content;
        entity.UsageCount = dto.UsageCount;
        entity.Variables = dto.Variables?.ToList() ?? new List<string>();
        await _repo.UpdateAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        if (entity == null) return;
        await _repo.DeleteAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);
    }
}
