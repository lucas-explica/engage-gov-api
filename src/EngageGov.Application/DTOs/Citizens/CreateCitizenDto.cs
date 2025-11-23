namespace EngageGov.Application.DTOs.Citizens;

/// <summary>
/// DTO for creating a new citizen
/// </summary>
public class CreateCitizenDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Neighborhood { get; set; }
    public int Points { get; set; } = 0;
}
