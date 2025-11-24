using EngageGov.Application.DTOs.Letters;
using EngageGov.Domain.Entities;
using System.Threading;

namespace EngageGov.Application.Interfaces;

public interface ILetterService
{
    Task<IEnumerable<LetterDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LetterDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<LetterDto> CreateAsync(LetterDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(LetterDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
