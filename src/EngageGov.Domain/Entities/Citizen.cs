using EngageGov.Domain.Common;

namespace EngageGov.Domain.Entities;

/// <summary>
/// Represents a citizen user in the system
/// </summary>
public class Citizen : BaseEntity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Neighborhood { get; private set; }
    public int Points { get; private set; }

    // Removido: propriedades de navegação obrigatórias

    private Citizen()
    {
        Name = string.Empty;
        Email = string.Empty;
        Phone = string.Empty;
        Neighborhood = string.Empty;
        Points = 0;
    }

    public Citizen(string name, string email, string? phone = null, string? neighborhood = null, int points = 0)
        : base()
    {
        Name = name;
        Email = email.ToLowerInvariant();
        Phone = phone;
        Neighborhood = neighborhood;
        Points = points;
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (!email.Contains('@') || !email.Contains('.'))
            throw new ArgumentException("Invalid email format", nameof(email));

        if (email.Length > 255)
            throw new ArgumentException("Email cannot exceed 255 characters", nameof(email));
    }

}
