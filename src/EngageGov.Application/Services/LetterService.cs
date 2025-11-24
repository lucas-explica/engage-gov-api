using EngageGov.Application.DTOs.Letters;
using EngageGov.Application.Interfaces;
using EngageGov.Domain.Entities;

namespace EngageGov.Application.Services;

public class LetterService : ILetterService
{
    private readonly ILetterRepository _repository;

    public LetterService(ILetterRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<LetterDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var letters = await _repository.GetAllAsync(cancellationToken);
        return letters.Select(l => new LetterDto(l.Id, l.Title, l.Status, l.CreatedAt));
    }

    public async Task<LetterDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var letter = await _repository.GetByIdAsync(id, cancellationToken);
        return letter == null ? null : new LetterDto(letter.Id, letter.Title, letter.Status, letter.CreatedAt);
    }

    public async Task<LetterDto> CreateAsync(LetterDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new Letter
        {
            Id = dto.Id,
            Title = dto.Title,
            Status = dto.Status,
            CreatedAt = dto.CreatedAt
        };
        await _repository.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return new LetterDto(entity.Id, entity.Title, entity.Status, entity.CreatedAt);
    }

    public async Task UpdateAsync(LetterDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(dto.Id, cancellationToken);
        if (entity == null) return;
        entity.Title = dto.Title;
        entity.Status = dto.Status;
        entity.CreatedAt = dto.CreatedAt;
        await _repository.UpdateAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return;
        await _repository.DeleteAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
