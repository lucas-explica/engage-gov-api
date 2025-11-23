using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EngageGov.Application.DTOs;
using EngageGov.Application.Interfaces;

namespace EngageGov.Application.Services
{
    public class SenadoGovService : IGovernmentDataService
    {
        private readonly HttpClient _http;

        public SenadoGovService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<RepresentativeDto>> GetRepresentativesAsync(string source, CancellationToken cancellationToken = default)
        {
            // Attempt common endpoints; be defensive as API surface may differ
            try
            {
                // Try a set of likely endpoints used by the Senado API UI
                var candidates = new[] { "/api/v1/senadores", "/dadosabertos/senadores", "/senadores" };
                HttpResponseMessage? res = null;
                foreach (var c in candidates)
                {
                    // prefer JSON: some Senado endpoints use formato=json query param
                    var url = c.Contains('?') ? c + "&formato=json" : c + "?formato=json";
                    res = await _http.GetAsync(url, cancellationToken);
                    if (res.IsSuccessStatusCode) break;
                    // fallback to raw endpoint without param
                    res = await _http.GetAsync(c, cancellationToken);
                    if (res.IsSuccessStatusCode) break;
                }
                if (res == null || !res.IsSuccessStatusCode) return Array.Empty<RepresentativeDto>();
                if (!res.IsSuccessStatusCode) return Array.Empty<RepresentativeDto>();
                using var s = await res.Content.ReadAsStreamAsync(cancellationToken);
                var doc = await JsonDocument.ParseAsync(s, cancellationToken: cancellationToken);
                var list = new List<RepresentativeDto>();
                if (doc.RootElement.TryGetProperty("dados", out var dados) && dados.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in dados.EnumerateArray())
                    {
                        var externalId = item.GetProperty("nrMatricula").GetRawText().Trim('"');
                        var name = item.GetProperty("txNomeParlamentar").GetString() ?? string.Empty;
                        var party = item.TryGetProperty("sgPartido", out var p) ? p.GetString() ?? string.Empty : string.Empty;
                        var state = item.TryGetProperty("sgUF", out var uf) ? uf.GetString() ?? string.Empty : string.Empty;
                        var photo = string.Empty;
                        var id = GenerateId("senado", externalId);
                        list.Add(new RepresentativeDto(id, externalId, "senado", name, party, state, photo));
                    }
                }
                return list;
            }
            catch
            {
                return Array.Empty<RepresentativeDto>();
            }
        }

        public async Task<IEnumerable<LawDto>> GetLawsAsync(string source, int? year = null, int items = 20, CancellationToken cancellationToken = default)
        {
            try
            {
                var candidates = new[] { "/api/v1/proposicoes", "/dadosabertos/proposicoes", "/proposicoes" };
                HttpResponseMessage? res = null;
                foreach (var c in candidates)
                {
                    var qs = year.HasValue ? $"ano={year.Value}&itens={items}" : $"itens={items}";
                    var urlWithParam = c.Contains('?') ? c + "&" + qs : c + "?" + qs;
                    // try prefer JSON
                    var urlJson = urlWithParam + "&formato=json";
                    res = await _http.GetAsync(urlJson, cancellationToken);
                    if (res.IsSuccessStatusCode) break;
                    // fallback to non-json param
                    res = await _http.GetAsync(urlWithParam, cancellationToken);
                    if (res.IsSuccessStatusCode) break;
                }
                if (res == null || !res.IsSuccessStatusCode) return Array.Empty<LawDto>();
                if (!res.IsSuccessStatusCode) return Array.Empty<LawDto>();
                using var s = await res.Content.ReadAsStreamAsync(cancellationToken);
                var doc = await JsonDocument.ParseAsync(s, cancellationToken: cancellationToken);
                var list = new List<LawDto>();
                if (doc.RootElement.TryGetProperty("dados", out var dados) && dados.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in dados.EnumerateArray())
                    {
                        var externalId = item.GetProperty("id").GetRawText().Trim('"');
                        var type = item.TryGetProperty("sigla", out var st) ? st.GetString() ?? string.Empty : string.Empty;
                        var number = item.TryGetProperty("numero", out var num) ? num.GetRawText().Trim('"') : string.Empty;
                        var y = item.TryGetProperty("ano", out var ay) && ay.ValueKind == JsonValueKind.Number ? ay.GetInt32() : (int?)null;
                        var ementa = item.TryGetProperty("ementa", out var em) ? em.GetString() : string.Empty;
                        var id = GenerateId("senado", externalId);
                        list.Add(new LawDto(id, externalId, "senado", type ?? string.Empty, number ?? string.Empty, y, ementa ?? string.Empty, string.Empty, null, null, string.Empty, string.Empty));
                    }
                }
                return list;
            }
            catch
            {
                return Array.Empty<LawDto>();
            }
        }

        public Task<LawDto?> GetLawByExternalIdAsync(string source, string externalId, CancellationToken cancellationToken = default)
        {
            // Not yet implemented per detailed mapping unavailability; return null
            return Task.FromResult<LawDto?>(null);
        }


        public Task<IEnumerable<SpeechDto>> GetSpeechesAsync(string source, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<SpeechDto>>(Array.Empty<SpeechDto>());
        }

        private static string GenerateId(string source, string externalId)
            => System.Convert.ToHexString(System.Security.Cryptography.MD5.HashData(System.Text.Encoding.UTF8.GetBytes(source + ":" + externalId))).ToLowerInvariant();
    }
}
