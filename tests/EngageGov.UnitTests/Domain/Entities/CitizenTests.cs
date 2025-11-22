using EngageGov.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace EngageGov.UnitTests.Domain.Entities;

/// <summary>
/// Unit tests for Citizen entity
/// Tests domain business rules and validation
/// </summary>
public class CitizenTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateCitizen()
    {
        // Arrange
        var fullName = "John Doe";
        var email = "john.doe@example.com";
        var documentNumber = "12345678901";
        var phoneNumber = "+1234567890";

        // Act
        var citizen = new Citizen(fullName, email, documentNumber, phoneNumber);

        // Assert
        citizen.Should().NotBeNull();
        citizen.FullName.Should().Be(fullName);
        citizen.Email.Should().Be(email.ToLowerInvariant());
        citizen.DocumentNumber.Should().Be(documentNumber);
        citizen.PhoneNumber.Should().Be(phoneNumber);
        citizen.IsEmailVerified.Should().BeFalse();
        citizen.IsActive.Should().BeTrue();
        citizen.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Jo")] // Too short
    public void Constructor_WithInvalidFullName_ShouldThrowArgumentException(string invalidName)
    {
        // Act & Assert
        Action act = () => new Citizen(invalidName, "test@example.com", "12345678901");
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("invalid-email")]
    [InlineData("missing@domain")]
    public void Constructor_WithInvalidEmail_ShouldThrowArgumentException(string invalidEmail)
    {
        // Act & Assert
        Action act = () => new Citizen("John Doe", invalidEmail, "12345678901");
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1234")] // Too short
    public void Constructor_WithInvalidDocumentNumber_ShouldThrowArgumentException(string invalidDoc)
    {
        // Act & Assert
        Action act = () => new Citizen("John Doe", "test@example.com", invalidDoc);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UpdateEmail_WithValidEmail_ShouldUpdateAndResetVerification()
    {
        // Arrange
        var citizen = CreateValidCitizen();
        citizen.VerifyEmail(); // Verify first
        var newEmail = "newemail@example.com";

        // Act
        citizen.UpdateEmail(newEmail);

        // Assert
        citizen.Email.Should().Be(newEmail.ToLowerInvariant());
        citizen.IsEmailVerified.Should().BeFalse(); // Should be reset
    }

    [Fact]
    public void VerifyEmail_ShouldSetVerificationToTrue()
    {
        // Arrange
        var citizen = CreateValidCitizen();

        // Act
        citizen.VerifyEmail();

        // Assert
        citizen.IsEmailVerified.Should().BeTrue();
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var citizen = CreateValidCitizen();

        // Act
        citizen.Deactivate();

        // Assert
        citizen.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Activate_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var citizen = CreateValidCitizen();
        citizen.Deactivate(); // First deactivate

        // Act
        citizen.Activate();

        // Assert
        citizen.IsActive.Should().BeTrue();
    }

    [Fact]
    public void UpdateProfile_WithValidData_ShouldUpdateFields()
    {
        // Arrange
        var citizen = CreateValidCitizen();
        var newName = "Jane Smith";
        var newPhone = "+9876543210";

        // Act
        citizen.UpdateProfile(newName, newPhone);

        // Assert
        citizen.FullName.Should().Be(newName);
        citizen.PhoneNumber.Should().Be(newPhone);
    }

    private static Citizen CreateValidCitizen()
    {
        return new Citizen(
            "John Doe",
            "john.doe@example.com",
            "12345678901",
            "+1234567890"
        );
    }
}
