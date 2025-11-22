namespace EngageGov.Domain.Enums;

/// <summary>
/// Status of a civic proposal
/// </summary>
public enum ProposalStatus
{
    Draft = 0,
    UnderReview = 1,
    InProgress = 2,
    Completed = 3,
    Rejected = 4,
    Archived = 5
}
