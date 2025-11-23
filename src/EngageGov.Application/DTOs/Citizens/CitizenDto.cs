namespace EngageGov.Application.DTOs.Citizens;

/// <summary>
/// DTO for citizen response
/// </summary>
public class CitizenDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Neighborhood { get; set; }
    public int Points { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
