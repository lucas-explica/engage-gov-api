using EngageGov.Domain.Enums;

namespace EngageGov.Application.DTOs.Proposals;

/// <summary>
/// DTO for updating an existing proposal
/// </summary>
public class UpdateProposalDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public PriorityLevel? Priority { get; set; }
    public ProposalStatus? Status { get; set; }
    public decimal? EstimatedCost { get; set; }
    public DateTime? TargetCompletionDate { get; set; }
}
