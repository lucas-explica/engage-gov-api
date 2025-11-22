using EngageGov.Domain.Entities;
using EngageGov.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace EngageGov.UnitTests.Domain.Entities;

/// <summary>
/// Unit tests for Proposal entity
/// Tests domain business rules and validation
/// </summary>
public class ProposalTests
{
    private static readonly Guid TestCitizenId = Guid.NewGuid();

    [Fact]
    public void Constructor_WithValidData_ShouldCreateProposal()
    {
        // Arrange
        var title = "Fix the Main Street Pothole";
        var description = "There is a dangerous pothole on Main Street that needs immediate attention";
        var type = ProposalType.Infrastructure;
        var location = "Main Street, Downtown";

        // Act
        var proposal = new Proposal(title, description, type, TestCitizenId, location);

        // Assert
        proposal.Should().NotBeNull();
        proposal.Title.Should().Be(title);
        proposal.Description.Should().Be(description);
        proposal.Type.Should().Be(type);
        proposal.CitizenId.Should().Be(TestCitizenId);
        proposal.Location.Should().Be(location);
        proposal.Status.Should().Be(ProposalStatus.Draft);
        proposal.Priority.Should().Be(PriorityLevel.Medium);
        proposal.VoteCount.Should().Be(0);
        proposal.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Test")] // Too short
    public void Constructor_WithInvalidTitle_ShouldThrowArgumentException(string invalidTitle)
    {
        // Arrange
        var description = "This is a valid description with enough characters";
        var type = ProposalType.Infrastructure;
        var location = "Main Street";

        // Act & Assert
        Action act = () => new Proposal(invalidTitle, description, type, TestCitizenId, location);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Too short")] // Too short
    public void Constructor_WithInvalidDescription_ShouldThrowArgumentException(string invalidDescription)
    {
        // Arrange
        var title = "Valid Title Here";
        var type = ProposalType.Infrastructure;
        var location = "Main Street";

        // Act & Assert
        Action act = () => new Proposal(title, invalidDescription, type, TestCitizenId, location);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UpdateStatus_WithNewStatus_ShouldUpdateStatusAndTimestamp()
    {
        // Arrange
        var proposal = CreateValidProposal();
        var originalUpdatedAt = proposal.UpdatedAt;

        // Act
        proposal.UpdateStatus(ProposalStatus.InProgress);

        // Assert
        proposal.Status.Should().Be(ProposalStatus.InProgress);
        proposal.UpdatedAt.Should().NotBe(originalUpdatedAt);
    }

    [Fact]
    public void IncrementVoteCount_ShouldIncreaseVoteCount()
    {
        // Arrange
        var proposal = CreateValidProposal();
        var originalCount = proposal.VoteCount;

        // Act
        proposal.IncrementVoteCount();

        // Assert
        proposal.VoteCount.Should().Be(originalCount + 1);
    }

    [Fact]
    public void DecrementVoteCount_WhenCountIsPositive_ShouldDecreaseVoteCount()
    {
        // Arrange
        var proposal = CreateValidProposal();
        proposal.IncrementVoteCount();
        var originalCount = proposal.VoteCount;

        // Act
        proposal.DecrementVoteCount();

        // Assert
        proposal.VoteCount.Should().Be(originalCount - 1);
    }

    [Fact]
    public void DecrementVoteCount_WhenCountIsZero_ShouldNotGoBelowZero()
    {
        // Arrange
        var proposal = CreateValidProposal();

        // Act
        proposal.DecrementVoteCount();

        // Assert
        proposal.VoteCount.Should().Be(0);
    }

    [Fact]
    public void SetEstimatedCost_WithPositiveValue_ShouldSetCost()
    {
        // Arrange
        var proposal = CreateValidProposal();
        var cost = 15000.50m;

        // Act
        proposal.SetEstimatedCost(cost);

        // Assert
        proposal.EstimatedCost.Should().Be(cost);
    }

    [Fact]
    public void SetEstimatedCost_WithNegativeValue_ShouldThrowArgumentException()
    {
        // Arrange
        var proposal = CreateValidProposal();

        // Act & Assert
        Action act = () => proposal.SetEstimatedCost(-100);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void SetTargetCompletionDate_WithFutureDate_ShouldSetDate()
    {
        // Arrange
        var proposal = CreateValidProposal();
        var futureDate = DateTime.UtcNow.AddDays(30);

        // Act
        proposal.SetTargetCompletionDate(futureDate);

        // Assert
        proposal.TargetCompletionDate.Should().Be(futureDate);
    }

    [Fact]
    public void SetTargetCompletionDate_WithPastDate_ShouldThrowArgumentException()
    {
        // Arrange
        var proposal = CreateValidProposal();
        var pastDate = DateTime.UtcNow.AddDays(-1);

        // Act & Assert
        Action act = () => proposal.SetTargetCompletionDate(pastDate);
        act.Should().Throw<ArgumentException>();
    }

    private static Proposal CreateValidProposal()
    {
        return new Proposal(
            "Fix Main Street Pothole",
            "There is a dangerous pothole on Main Street that needs immediate attention for public safety",
            ProposalType.Infrastructure,
            TestCitizenId,
            "Main Street, Downtown"
        );
    }
}
