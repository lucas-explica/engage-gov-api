using EngageGov.Application.DTOs.Citizens;
using EngageGov.Application.DTOs.Common;

namespace EngageGov.Application.Interfaces;

/// <summary>
/// Service interface for citizen operations
/// </summary>
public interface ICitizenService
{
    Task<Result<CitizenDto>> CreateCitizenAsync(CreateCitizenDto dto, CancellationToken cancellationToken = default);
    Task<Result<CitizenDto>> GetCitizenByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<CitizenDto>> GetCitizenByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CitizenDto>>> GetAllCitizensAsync(CancellationToken cancellationToken = default);
    Task<Result> DeleteCitizenAsync(Guid id, CancellationToken cancellationToken = default);
}
