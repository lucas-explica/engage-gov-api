using EngageGov.Domain.Common;

namespace EngageGov.Domain.Entities;

/// <summary>
/// Represents a citizen user in the system
/// </summary>
public class Citizen : BaseEntity
{
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string DocumentNumber { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public ICollection<Proposal> Proposals { get; private set; } = new List<Proposal>();
    public ICollection<Vote> Votes { get; private set; } = new List<Vote>();
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();

    // Private constructor for EF Core
    private Citizen()
    {
        FullName = string.Empty;
        Email = string.Empty;
        DocumentNumber = string.Empty;
    }

    public Citizen(string fullName, string email, string documentNumber, string? phoneNumber = null)
        : base()
    {
        ValidateFullName(fullName);
        ValidateEmail(email);
        ValidateDocumentNumber(documentNumber);

        FullName = fullName;
        Email = email.ToLowerInvariant();
        DocumentNumber = documentNumber;
        PhoneNumber = phoneNumber;
        IsEmailVerified = false;
        IsActive = true;
    }

    public void UpdateProfile(string fullName, string? phoneNumber)
    {
        ValidateFullName(fullName);

        FullName = fullName;
        PhoneNumber = phoneNumber;
        SetUpdatedAt();
    }

    public void UpdateEmail(string email)
    {
        ValidateEmail(email);

        Email = email.ToLowerInvariant();
        IsEmailVerified = false;
        SetUpdatedAt();
    }

    public void VerifyEmail()
    {
        IsEmailVerified = true;
        SetUpdatedAt();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    private static void ValidateFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty", nameof(fullName));

        if (fullName.Length < 3)
            throw new ArgumentException("Full name must be at least 3 characters long", nameof(fullName));

        if (fullName.Length > 200)
            throw new ArgumentException("Full name cannot exceed 200 characters", nameof(fullName));
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

    private static void ValidateDocumentNumber(string documentNumber)
    {
        if (string.IsNullOrWhiteSpace(documentNumber))
            throw new ArgumentException("Document number cannot be empty", nameof(documentNumber));

        if (documentNumber.Length < 5)
            throw new ArgumentException("Document number must be at least 5 characters long", nameof(documentNumber));

        if (documentNumber.Length > 50)
            throw new ArgumentException("Document number cannot exceed 50 characters", nameof(documentNumber));
    }
}
