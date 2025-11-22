using EngageGov.Domain.Common;

namespace EngageGov.Domain.Entities;

/// <summary>
/// Represents a vote on a proposal
/// </summary>
public class Vote : BaseEntity
{
    public Guid ProposalId { get; private set; }
    public Guid CitizenId { get; private set; }
    public bool IsUpvote { get; private set; }

    // Navigation properties
    public Proposal Proposal { get; private set; } = null!;
    public Citizen Citizen { get; private set; } = null!;

    // Private constructor for EF Core
    private Vote() { }

    public Vote(Guid proposalId, Guid citizenId, bool isUpvote = true)
        : base()
    {
        ProposalId = proposalId;
        CitizenId = citizenId;
        IsUpvote = isUpvote;
    }

    public void ToggleVote()
    {
        IsUpvote = !IsUpvote;
        SetUpdatedAt();
    }
}
