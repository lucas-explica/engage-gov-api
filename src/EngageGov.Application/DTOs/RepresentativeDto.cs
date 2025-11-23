namespace EngageGov.Application.DTOs
{
    public record RepresentativeDto
    (
        string Id,
        string ExternalId,
        string Source,
        string Name,
        string Party,
        string State,
        string PhotoUrl
    );
}
