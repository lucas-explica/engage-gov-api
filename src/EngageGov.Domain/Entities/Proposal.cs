using EngageGov.Domain.Common;
using EngageGov.Domain.Enums;

namespace EngageGov.Domain.Entities;

/// <summary>
/// Represents a civic engagement proposal submitted by citizens
/// </summary>
public class Proposal : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public ProposalType Type { get; private set; }
    public ProposalStatus Status { get; private set; }
    public PriorityLevel Priority { get; private set; }
    public Guid CitizenId { get; private set; }
    public string Location { get; private set; }
    public decimal? EstimatedCost { get; private set; }
    public DateTime? TargetCompletionDate { get; private set; }
    public int VoteCount { get; private set; }

    // Navigation property
    public Citizen Citizen { get; private set; } = null!;
    public ICollection<Vote> Votes { get; private set; } = new List<Vote>();
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();

    // Private constructor for EF Core
    private Proposal() 
    { 
        Title = string.Empty;
        Description = string.Empty;
        Location = string.Empty;
    }

    public Proposal(
        string title, 
        string description, 
        ProposalType type,
        Guid citizenId,
        string location,
        PriorityLevel priority = PriorityLevel.Medium)
        : base()
    {
        ValidateTitle(title);
        ValidateDescription(description);
        ValidateLocation(location);

        Title = title;
        Description = description;
        Type = type;
        CitizenId = citizenId;
        Location = location;
        Priority = priority;
        Status = ProposalStatus.Draft;
        VoteCount = 0;
    }

    public void UpdateDetails(string title, string description, string location)
    {
        ValidateTitle(title);
        ValidateDescription(description);
        ValidateLocation(location);

        Title = title;
        Description = description;
        Location = location;
        SetUpdatedAt();
    }

    public void UpdateStatus(ProposalStatus newStatus)
    {
        if (Status == newStatus)
            return;

        Status = newStatus;
        SetUpdatedAt();
    }

    public void UpdatePriority(PriorityLevel newPriority)
    {
        if (Priority == newPriority)
            return;

        Priority = newPriority;
        SetUpdatedAt();
    }

    public void SetEstimatedCost(decimal cost)
    {
        if (cost < 0)
            throw new ArgumentException("Estimated cost cannot be negative", nameof(cost));

        EstimatedCost = cost;
        SetUpdatedAt();
    }

    public void SetTargetCompletionDate(DateTime targetDate)
    {
        if (targetDate < DateTime.UtcNow)
            throw new ArgumentException("Target completion date cannot be in the past", nameof(targetDate));

        TargetCompletionDate = targetDate;
        SetUpdatedAt();
    }

    public void IncrementVoteCount()
    {
        VoteCount++;
        SetUpdatedAt();
    }

    public void DecrementVoteCount()
    {
        if (VoteCount > 0)
            VoteCount--;
        SetUpdatedAt();
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (title.Length < 5)
            throw new ArgumentException("Title must be at least 5 characters long", nameof(title));

        if (title.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters", nameof(title));
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        if (description.Length < 20)
            throw new ArgumentException("Description must be at least 20 characters long", nameof(description));

        if (description.Length > 5000)
            throw new ArgumentException("Description cannot exceed 5000 characters", nameof(description));
    }

    private static void ValidateLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Location cannot be empty", nameof(location));

        if (location.Length > 500)
            throw new ArgumentException("Location cannot exceed 500 characters", nameof(location));
    }
}
