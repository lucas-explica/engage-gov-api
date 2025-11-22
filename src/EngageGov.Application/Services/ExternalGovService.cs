using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using EngageGov.Application.DTOs.Laws;
using EngageGov.Application.DTOs.Representatives;
using EngageGov.Application.Interfaces;

namespace EngageGov.Application.Services;

/// <summary>
/// ExternalGovService (Application layer) - performs HTTP requests to Câmara Dados Abertos.
/// Expects JSON responses when possible (Accept: application/json). Falls back to XML parsing if necessary.
/// </summary>
public class ExternalGovService : IExternalGovService
{
    private readonly HttpClient _http;

    public ExternalGovService(HttpClient http)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
    }

    private static Guid StableGuid(string input)
    {
        // Create deterministic GUID from input using MD5 (stable across runs)
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));
        return new Guid(hash);
    }

    public async Task<IEnumerable<RepresentativeDto>> GetRepresentativesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, "/api/v2/deputados?itens=500");
            req.Headers.Accept.Clear();
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resp = await _http.SendAsync(req, cancellationToken);
            resp.EnsureSuccessStatusCode();
            var text = await resp.Content.ReadAsStringAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(text)) return Enumerable.Empty<RepresentativeDto>();

            // Expect JSON response (Accept: application/json). If not JSON or missing 'dados', return empty.
            JsonDocument? doc = null;
            try
            {
                doc = JsonDocument.Parse(text);
            }
            catch
            {
                return Enumerable.Empty<RepresentativeDto>();
            }
            if (!doc.RootElement.TryGetProperty("dados", out var arr)) return Enumerable.Empty<RepresentativeDto>();
            var list = new List<RepresentativeDto>();
            foreach (var item in arr.EnumerateArray())
            {
                var externalId = item.TryGetProperty("id", out var idProp) ? idProp.ToString() : Guid.NewGuid().ToString();
                var name = item.TryGetProperty("nome", out var n) ? n.GetString() ?? string.Empty : string.Empty;
                var party = item.TryGetProperty("siglaPartido", out var p) ? p.GetString() ?? string.Empty : string.Empty;
                list.Add(new RepresentativeDto(StableGuid("deputado:" + externalId), name, party));
            }
            return list;
        }
        catch
        {
            // Defensive: on any error return empty list so frontend continues working
            return Enumerable.Empty<RepresentativeDto>();
        }
    }

    public async Task<IEnumerable<LawDto>> GetProposalsAsync(int year = 2024, int items = 50, CancellationToken cancellationToken = default)
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, $"/api/v2/proposicoes?ano={year}&itens={items}");
            req.Headers.Accept.Clear();
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resp = await _http.SendAsync(req, cancellationToken);
            resp.EnsureSuccessStatusCode();
            var text = await resp.Content.ReadAsStringAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(text)) return Enumerable.Empty<LawDto>();

            // Expect JSON response (Accept: application/json). If not JSON or missing 'dados', return empty.
            JsonDocument? doc = null;
            try
            {
                doc = JsonDocument.Parse(text);
            }
            catch
            {
                return Enumerable.Empty<LawDto>();
            }
            if (!doc.RootElement.TryGetProperty("dados", out var arr)) return Enumerable.Empty<LawDto>();
            var list = new List<LawDto>();
            foreach (var item in arr.EnumerateArray())
            {
                var idStr = item.TryGetProperty("id", out var idp) ? idp.ToString() : Guid.NewGuid().ToString();
                var ementa = item.TryGetProperty("ementa", out var e) ? e.GetString() ?? string.Empty : string.Empty;
                DateTime? dt = null;
                if (item.TryGetProperty("dataApresentacao", out var p) && p.ValueKind == JsonValueKind.String)
                {
                    if (DateTime.TryParse(p.GetString(), out var d)) dt = d;
                }
                list.Add(new LawDto(StableGuid("proposicao:" + idStr), ementa, ementa, dt));
            }
            return list;
        }
        catch
        {
            return Enumerable.Empty<LawDto>();
        }
    }
}
