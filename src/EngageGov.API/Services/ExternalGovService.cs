using EngageGov.Application.DTOs.Laws;
using EngageGov.Application.DTOs.Representatives;
using EngageGov.Application.Interfaces;
using System.Net.Http.Json;

namespace EngageGov.API.Services;

public class ExternalGovService : IExternalGovService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ExternalGovService> _logger;

    public ExternalGovService(IHttpClientFactory httpClientFactory, ILogger<ExternalGovService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<RepresentativeDto>> GetRepresentativesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("camara");
            var resp = await client.GetAsync("/api/v2/deputados?itens=500", cancellationToken);
            resp.EnsureSuccessStatusCode();
            var text = await resp.Content.ReadAsStringAsync(cancellationToken);

            // Prefer JSON if provided
            if (text.TrimStart().StartsWith("<"))
            {
                var xdoc = System.Xml.Linq.XDocument.Parse(text);
                var deputies = xdoc.Descendants("deputado_");
                return deputies.Select(d => new RepresentativeDto(
                    Id: Guid.TryParse((string?)d.Element("id"), out var g) ? g : Guid.NewGuid(),
                    Name: (string?)d.Element("nome") ?? string.Empty,
                    Party: (string?)d.Element("siglaPartido") ?? string.Empty
                )).ToList();
            }

            var json = System.Text.Json.JsonDocument.Parse(text);
            var data = json.RootElement.GetProperty("dados");
            var list = new List<RepresentativeDto>();
            foreach (var item in data.EnumerateArray())
            {
                var idStr = item.GetProperty("id").GetString();
                Guid id = Guid.NewGuid();
                if (!string.IsNullOrEmpty(idStr) && Guid.TryParse(idStr, out var parsed)) id = parsed;
                list.Add(new RepresentativeDto(id, item.GetProperty("nome").GetString() ?? string.Empty, item.GetProperty("siglaPartido").GetString() ?? string.Empty));
            }
            return list;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "External Gov: failed to fetch representatives");
            return Enumerable.Empty<RepresentativeDto>();
        }
    }

    public async Task<IEnumerable<LawDto>> GetProposalsAsync(int year = 2024, int items = 50, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("camara");
            var resp = await client.GetAsync($"/api/v2/proposicoes?ano={year}&itens={items}", cancellationToken);
            resp.EnsureSuccessStatusCode();
            var text = await resp.Content.ReadAsStringAsync(cancellationToken);

            if (text.TrimStart().StartsWith("<"))
            {
                var xdoc = System.Xml.Linq.XDocument.Parse(text);
                var props = xdoc.Descendants("proposicao_");
                return props.Select(p => new LawDto(
                    Id: Guid.TryParse((string?)p.Element("id"), out var g) ? g : Guid.NewGuid(),
                    Title: (string?)p.Element("ementa") ?? string.Empty,
                    Summary: (string?)p.Element("ementa") ?? string.Empty,
                    PresentedAt: DateTime.TryParse((string?)p.Element("dataApresentacao"), out var dt) ? dt : (DateTime?)null
                )).ToList();
            }

            var json = System.Text.Json.JsonDocument.Parse(text);
            var data = json.RootElement.GetProperty("dados");
            var list = new List<LawDto>();
            foreach (var item in data.EnumerateArray())
            {
                DateTime? dt = null;
                if (item.TryGetProperty("dataApresentacao", out var p) && p.ValueKind == System.Text.Json.JsonValueKind.String)
                {
                    if (DateTime.TryParse(p.GetString(), out var d)) dt = d;
                }
                list.Add(new LawDto(
                    Id: item.TryGetProperty("id", out var idp) && idp.ValueKind == System.Text.Json.JsonValueKind.Number ? Guid.NewGuid() : (Guid?)null,
                    Title: item.GetProperty("ementa").GetString() ?? string.Empty,
                    Summary: item.GetProperty("ementa").GetString() ?? string.Empty,
                    PresentedAt: dt
                ));
            }
            return list;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "External Gov: failed to fetch proposals");
            return Enumerable.Empty<LawDto>();
        }
    }
}
