using EngageGov.Domain.Common;
using EngageGov.Domain.Enums;

namespace EngageGov.Domain.Entities;

/// <summary>
/// Represents a comment on a proposal
/// </summary>
public class Comment : BaseEntity
{
    public Guid ProposalId { get; private set; }
    public Guid CitizenId { get; private set; }
    public string Content { get; private set; }
    public CommentSentiment Sentiment { get; private set; } = CommentSentiment.Neutral;

    // Removido: propriedades de navegação obrigatórias

    // Private constructor for EF Core
    private Comment()
    {
        Content = string.Empty;
    }

    public Comment(Guid proposalId, Guid citizenId, string content, CommentSentiment sentiment)
        : base()
    {
        ProposalId = proposalId;
        CitizenId = citizenId;
        Content = content;
        Sentiment = sentiment;
    }

    public void UpdateContent(string content, CommentSentiment sentiment)
    {
        Content = content;
        Sentiment = sentiment;
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
