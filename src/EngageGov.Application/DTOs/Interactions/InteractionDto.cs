namespace EngageGov.Application.DTOs.Interactions;

public record InteractionDto
(
    System.Guid Id,
    string Type,
    System.DateTime CreatedAt
);
