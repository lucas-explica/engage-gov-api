using EngageGov.Application.DTOs.Representatives;
using EngageGov.Application.DTOs.Laws;

namespace EngageGov.Application.Interfaces;

public interface IExternalGovService
{
    Task<IEnumerable<RepresentativeDto>> GetRepresentativesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<LawDto>> GetProposalsAsync(int year = 2024, int items = 50, CancellationToken cancellationToken = default);
}
