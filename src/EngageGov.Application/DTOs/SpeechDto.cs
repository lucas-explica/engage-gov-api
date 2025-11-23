using System;

namespace EngageGov.Application.DTOs
{
    public record SpeechDto(
        string Id,
        string ExternalId,
        string Source,
        string RepresentativeExternalId,
        DateTimeOffset Date,
        string SessionId,
        string Text
    );
}
