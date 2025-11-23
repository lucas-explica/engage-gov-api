using System.Collections.Generic;

namespace EngageGov.Application.DTOs
{
    public record LawDto
    (
        string Id,
        string ExternalId,
        string Source,
        string Type,
        string Number,
        int? Year,
        string Ementa,
        string Status,
        IEnumerable<string>? Authors,
        IEnumerable<TimelineItemDto>? Timeline,
        string Url,
        string PresentationDate
    );
}
