namespace EngageGov.Application.DTOs.Templates;

public record TemplateDto
(
    System.Guid Id,
    string Name,
    string Subject,
    string Body
);
