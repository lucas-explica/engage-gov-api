using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EngageGov.Application.DTOs;

namespace EngageGov.Application.Interfaces
{
    public interface IGovernmentDataService
    {
        Task<IEnumerable<RepresentativeDto>> GetRepresentativesAsync(string source, CancellationToken cancellationToken = default);
        Task<IEnumerable<LawDto>> GetLawsAsync(string source, int? year = null, int items = 20, CancellationToken cancellationToken = default);
        Task<LawDto?> GetLawByExternalIdAsync(string source, string externalId, CancellationToken cancellationToken = default);
        Task<IEnumerable<SpeechDto>> GetSpeechesAsync(string source, CancellationToken cancellationToken = default);
    }
}
