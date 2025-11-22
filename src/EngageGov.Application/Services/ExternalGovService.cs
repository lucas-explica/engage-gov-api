// Application-level stub for ExternalGovService. Implementation lives in API project where framework types are available.
using EngageGov.Application.DTOs.Laws;
using EngageGov.Application.DTOs.Representatives;
using EngageGov.Application.Interfaces;

namespace EngageGov.Application.Services;

public class ExternalGovService : IExternalGovService
{
    public Task<IEnumerable<RepresentativeDto>> GetRepresentativesAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(Enumerable.Empty<RepresentativeDto>());

    public Task<IEnumerable<LawDto>> GetProposalsAsync(int year = 2024, int items = 50, CancellationToken cancellationToken = default)
        => Task.FromResult(Enumerable.Empty<LawDto>());
}
