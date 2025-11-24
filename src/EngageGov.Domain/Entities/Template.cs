using EngageGov.Domain.Common;

namespace EngageGov.Domain.Entities;

public class Template : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int UsageCount { get; set; } = 0;
    public List<string> Variables { get; set; } = new();
}
