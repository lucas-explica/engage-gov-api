using EngageGov.Domain.Enums;

namespace EngageGov.Application.DTOs.Proposals;

/// <summary>
/// DTO for proposal response
/// </summary>
public class ProposalDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProposalType Type { get; set; }
    public ProposalStatus Status { get; set; }
    public PriorityLevel Priority { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal? EstimatedCost { get; set; }
    public DateTime? TargetCompletionDate { get; set; }
    public Guid CitizenId { get; set; }
    public string CitizenName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
