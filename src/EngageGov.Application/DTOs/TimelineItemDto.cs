using System;

namespace EngageGov.Application.DTOs
{
    public record TimelineItemDto(
        DateTimeOffset Date,
        string Event,
        string? Description
    );
}
