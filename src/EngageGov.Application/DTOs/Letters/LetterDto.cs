namespace EngageGov.Application.DTOs.Letters;

public record LetterDto
(
    System.Guid Id,
    string Title,
    string Status,
    System.DateTime CreatedAt
);
