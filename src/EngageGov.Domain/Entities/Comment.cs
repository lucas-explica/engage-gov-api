using EngageGov.Domain.Common;

namespace EngageGov.Domain.Entities;

/// <summary>
/// Represents a comment on a proposal
/// </summary>
public class Comment : BaseEntity
{
    public Guid ProposalId { get; private set; }
    public Guid CitizenId { get; private set; }
    public string Content { get; private set; }

    // Navigation properties
    public Proposal Proposal { get; private set; } = null!;
    public Citizen Citizen { get; private set; } = null!;

    // Private constructor for EF Core
    private Comment()
    {
        Content = string.Empty;
    }

    public Comment(Guid proposalId, Guid citizenId, string content)
        : base()
    {
        ValidateContent(content);

        ProposalId = proposalId;
        CitizenId = citizenId;
        Content = content;
    }

    public void UpdateContent(string content)
    {
        ValidateContent(content);

        Content = content;
        SetUpdatedAt();
    }

    private static void ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Comment content cannot be empty", nameof(content));

        if (content.Length < 3)
            throw new ArgumentException("Comment must be at least 3 characters long", nameof(content));

        if (content.Length > 2000)
            throw new ArgumentException("Comment cannot exceed 2000 characters", nameof(content));
    }
}
