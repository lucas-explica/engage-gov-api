namespace EngageGov.Application.DTOs.Citizens;

/// <summary>
/// DTO for creating a new citizen
/// </summary>
public class CreateCitizenDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}
