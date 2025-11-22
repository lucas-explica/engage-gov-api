namespace EngageGov.Application.DTOs.Laws;

public record LawDto
(
    System.Guid? Id,
    string Title,
    string Summary,
    System.DateTime? PresentedAt
);
