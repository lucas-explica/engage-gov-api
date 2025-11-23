using EngageGov.Domain.Entities;

namespace EngageGov.Domain.Interfaces;

/// <summary>
/// Repository interface for Citizen entity with specific query methods
/// </summary>
public interface ICitizenRepository : IRepository<Citizen>
{
    Task<Citizen?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    // Removed: GetByDocumentNumberAsync (property no longer exists)
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    // Removed: DocumentNumberExistsAsync (property no longer exists)
}
