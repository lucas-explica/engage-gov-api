using EngageGov.Domain.Entities;

namespace EngageGov.Domain.Interfaces;

/// <summary>
/// Repository interface for Citizen entity with specific query methods
/// </summary>
public interface ICitizenRepository : IRepository<Citizen>
{
    Task<Citizen?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Citizen?> GetByDocumentNumberAsync(string documentNumber, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> DocumentNumberExistsAsync(string documentNumber, CancellationToken cancellationToken = default);
}
