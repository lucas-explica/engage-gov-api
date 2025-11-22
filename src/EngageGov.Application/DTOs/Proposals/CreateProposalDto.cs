using EngageGov.Domain.Enums;

namespace EngageGov.Application.DTOs.Proposals;

/// <summary>
/// DTO for creating a new proposal
/// </summary>
public class CreateProposalDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProposalType Type { get; set; }
    public string Location { get; set; } = string.Empty;
    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
    public decimal? EstimatedCost { get; set; }
    public DateTime? TargetCompletionDate { get; set; }
}
