using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EngageGov.Application.DTOs;
using EngageGov.Application.Interfaces;

namespace EngageGov.Application.Services
{
    public class CamaraGovService : IGovernmentDataService
    {
        private readonly HttpClient _http;

        public CamaraGovService(HttpClient http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
    }

        public async Task<IEnumerable<RepresentativeDto>> GetRepresentativesAsync(string source, CancellationToken cancellationToken = default)
        {
            // source ignored for Camara implementation; assume camara
            var res = await _http.GetAsync("/api/v2/deputados", cancellationToken);
            res.EnsureSuccessStatusCode();
            using var s = await res.Content.ReadAsStreamAsync(cancellationToken);
            var doc = await JsonDocument.ParseAsync(s, cancellationToken: cancellationToken);
            var list = new List<RepresentativeDto>();
            if (doc.RootElement.TryGetProperty("dados", out var dados) && dados.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in dados.EnumerateArray())
                {
                    var externalId = item.GetProperty("id").GetRawText().Trim('"');
                    var name = item.GetProperty("nome").GetString() ?? string.Empty;
                    var party = item.GetProperty("siglaPartido").GetString() ?? string.Empty;
                    var state = item.GetProperty("siglaUf").GetString() ?? string.Empty;
                    var photo = item.TryGetProperty("urlFoto", out var p) ? p.GetString() : null;
                    var id = GenerateId("camara", externalId);
                    list.Add(new RepresentativeDto(id, externalId, "camara", name, party, state, photo ?? string.Empty));
                }
            }
            return list;
        }

        public async Task<IEnumerable<LawDto>> GetLawsAsync(string source, int? year = null, int items = 20, CancellationToken cancellationToken = default)
        {
            var url = $"/api/v2/proposicoes?itens={items}" + (year.HasValue ? $"&ano={year.Value}" : string.Empty);
            var res = await _http.GetAsync(url, cancellationToken);
            res.EnsureSuccessStatusCode();
            using var s = await res.Content.ReadAsStreamAsync(cancellationToken);
            var doc = await JsonDocument.ParseAsync(s, cancellationToken: cancellationToken);
            var list = new List<LawDto>();
            if (doc.RootElement.TryGetProperty("dados", out var dados) && dados.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in dados.EnumerateArray())
                {
                    var externalId = item.GetProperty("id").GetRawText().Trim('"');
                    var type = item.GetProperty("siglaTipo").GetString() ?? string.Empty;
                    var number = item.GetProperty("numero").GetRawText().Trim('"');
                    var y = item.TryGetProperty("ano", out var ay) && ay.ValueKind == JsonValueKind.Number ? ay.GetInt32() : (int?)null;
                    var ementa = item.TryGetProperty("ementa", out var em) ? em.GetString() : string.Empty;
                    var lawUrl = item.TryGetProperty("uri", out var uriProp) ? uriProp.GetString() : string.Empty;
                    var presentationDate = item.TryGetProperty("dataApresentacao", out var dateProp) ? dateProp.GetString() : string.Empty;
                    var id = GenerateId("camara", externalId);
                    list.Add(new LawDto(id, externalId, "camara", type ?? string.Empty, number, y, ementa ?? string.Empty, string.Empty, null, null, lawUrl ?? string.Empty, presentationDate ?? string.Empty));
                }
            }
            return list;
        }

        public Task<LawDto?> GetLawByExternalIdAsync(string source, string externalId, CancellationToken cancellationToken = default)
        {
            // Simple fetch by id
            return GetLawById(externalId, cancellationToken);
        }

        private async Task<LawDto?> GetLawById(string externalId, CancellationToken cancellationToken)
        {
            var res = await _http.GetAsync($"/api/v2/proposicoes/{externalId}", cancellationToken);
            if (!res.IsSuccessStatusCode) return null;
            using var s = await res.Content.ReadAsStreamAsync(cancellationToken);
            var doc = await JsonDocument.ParseAsync(s, cancellationToken: cancellationToken);
            if (doc.RootElement.TryGetProperty("dados", out var item))
            {
                var type = item.GetProperty("siglaTipo").GetString() ?? string.Empty;
                var number = item.GetProperty("numero").GetRawText().Trim('"');
                var y = item.TryGetProperty("ano", out var ay) && ay.ValueKind == JsonValueKind.Number ? ay.GetInt32() : (int?)null;
                var ementa = item.TryGetProperty("ementa", out var em) ? em.GetString() : string.Empty;
                var lawUrl = item.TryGetProperty("uri", out var uriProp) ? uriProp.GetString() : string.Empty;
                var presentationDate = item.TryGetProperty("dataApresentacao", out var dateProp) ? dateProp.GetString() : string.Empty;
                var id = GenerateId("camara", externalId);
                return new LawDto(id, externalId, "camara", type, number, y, ementa ?? string.Empty, string.Empty, null, null, lawUrl ?? string.Empty, presentationDate ?? string.Empty);
            }
            return null;
        }


        public Task<IEnumerable<SpeechDto>> GetSpeechesAsync(string source, CancellationToken cancellationToken = default)
        {
            // Not implemented in minimal service
            return Task.FromResult<IEnumerable<SpeechDto>>(Array.Empty<SpeechDto>());
        }

        private static string GenerateId(string source, string externalId)
            => System.Convert.ToHexString(System.Security.Cryptography.MD5.HashData(System.Text.Encoding.UTF8.GetBytes(source + ":" + externalId))).ToLowerInvariant();
    }
}
