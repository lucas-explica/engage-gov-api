namespace EngageGov.Application.DTOs.Templates;

public record TemplateDto(
    System.Guid Id,
    string Name,
    string Category,
    string Content,
    int UsageCount,
    string[] Variables
);

