using EngageGov.Domain.Common;

namespace EngageGov.Domain.Entities;

public class Letter : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "draft";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // Adicione outros campos conforme necessário
}
