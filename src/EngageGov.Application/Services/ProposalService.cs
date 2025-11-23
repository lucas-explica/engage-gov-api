using EngageGov.Application.DTOs.Common;
using EngageGov.Application.DTOs.Proposals;
using EngageGov.Application.Interfaces;
using EngageGov.Domain.Entities;
using EngageGov.Domain.Enums;
using EngageGov.Domain.Interfaces;

namespace EngageGov.Application.Services;

/// <summary>
/// Service implementation for proposal operations
/// Follows Single Responsibility Principle
/// </summary>
public class ProposalService : IProposalService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProposalService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<ProposalDto>> CreateProposalAsync(
        CreateProposalDto dto, 
        Guid citizenId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Verify citizen exists
            var citizen = await _unitOfWork.Citizens.GetByIdAsync(citizenId, cancellationToken);
            if (citizen == null)
                return Result<ProposalDto>.Failure("Citizen not found");


            // Create proposal entity
            var proposal = new Proposal(
                dto.Title,
                dto.Description,
                dto.Type,
                citizenId,
                dto.Location,
                dto.Priority
            );

            if (dto.EstimatedCost.HasValue)
                proposal.SetEstimatedCost(dto.EstimatedCost.Value);

            if (dto.TargetCompletionDate.HasValue)
                proposal.SetTargetCompletionDate(dto.TargetCompletionDate.Value);

            // Save to repository
            await _unitOfWork.Proposals.AddAsync(proposal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Map to DTO
            var proposalDto = MapToDto(proposal, citizen.Name);

            return Result<ProposalDto>.Success(proposalDto);
        }
        catch (ArgumentException ex)
        {
            return Result<ProposalDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result<ProposalDto>.Failure($"An error occurred while creating the proposal: {ex.Message}");
        }
    }

    public async Task<Result<ProposalDto>> GetProposalByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var proposal = await _unitOfWork.Proposals.GetByIdAsync(id, cancellationToken);
            if (proposal == null)
                return Result<ProposalDto>.Failure("Proposal not found");

            var citizen = await _unitOfWork.Citizens.GetByIdAsync(proposal.CitizenId, cancellationToken);
            var proposalDto = MapToDto(proposal, citizen?.Name ?? "Unknown");

            return Result<ProposalDto>.Success(proposalDto);
        }
        catch (Exception ex)
        {
            return Result<ProposalDto>.Failure($"An error occurred while retrieving the proposal: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<ProposalDto>>> GetAllProposalsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var proposals = await _unitOfWork.Proposals.GetAllAsync(cancellationToken);
            var proposalDtos = new List<ProposalDto>();
            foreach (var p in proposals)
            {
                var citizen = await _unitOfWork.Citizens.GetByIdAsync(p.CitizenId, cancellationToken);
                proposalDtos.Add(MapToDto(p, citizen?.Name ?? "Unknown"));
            }

            return Result<IEnumerable<ProposalDto>>.Success(proposalDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ProposalDto>>.Failure($"An error occurred while retrieving proposals: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<ProposalDto>>> GetProposalsByStatusAsync(
        ProposalStatus status, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var proposals = await _unitOfWork.Proposals.GetByStatusAsync(status, cancellationToken);
            var proposalDtos = new List<ProposalDto>();
            foreach (var p in proposals)
            {
                var citizen = await _unitOfWork.Citizens.GetByIdAsync(p.CitizenId, cancellationToken);
                proposalDtos.Add(MapToDto(p, citizen?.Name ?? "Unknown"));
            }

            return Result<IEnumerable<ProposalDto>>.Success(proposalDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ProposalDto>>.Failure($"An error occurred while retrieving proposals: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<ProposalDto>>> GetProposalsByCitizenAsync(
        Guid citizenId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var citizen = await _unitOfWork.Citizens.GetByIdAsync(citizenId, cancellationToken);
            if (citizen == null)
                return Result<IEnumerable<ProposalDto>>.Failure("Citizen not found");

            var proposals = await _unitOfWork.Proposals.GetByCitizenIdAsync(citizenId, cancellationToken);
            var proposalDtos = proposals.Select(p => MapToDto(p, citizen.Name));

            return Result<IEnumerable<ProposalDto>>.Success(proposalDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ProposalDto>>.Failure($"An error occurred while retrieving proposals: {ex.Message}");
        }
    }

    public async Task<Result<ProposalDto>> UpdateProposalAsync(
        Guid id, 
        UpdateProposalDto dto, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var proposal = await _unitOfWork.Proposals.GetByIdAsync(id, cancellationToken);
            if (proposal == null)
                return Result<ProposalDto>.Failure("Proposal not found");

            // Update basic details
            proposal.UpdateDetails(dto.Title, dto.Description, dto.Location);

            // Update optional fields
            if (dto.Priority.HasValue)
                proposal.UpdatePriority(dto.Priority.Value);

            if (dto.Status.HasValue)
                proposal.UpdateStatus(dto.Status.Value);

            if (dto.EstimatedCost.HasValue)
                proposal.SetEstimatedCost(dto.EstimatedCost.Value);

            if (dto.TargetCompletionDate.HasValue)
                proposal.SetTargetCompletionDate(dto.TargetCompletionDate.Value);

            await _unitOfWork.Proposals.UpdateAsync(proposal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var citizen = await _unitOfWork.Citizens.GetByIdAsync(proposal.CitizenId, cancellationToken);
            var proposalDto = MapToDto(proposal, citizen?.Name ?? "Unknown");

            return Result<ProposalDto>.Success(proposalDto);
        }
        catch (ArgumentException ex)
        {
            return Result<ProposalDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result<ProposalDto>.Failure($"An error occurred while updating the proposal: {ex.Message}");
        }
    }

    public async Task<Result> DeleteProposalAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var proposal = await _unitOfWork.Proposals.GetByIdAsync(id, cancellationToken);
            if (proposal == null)
                return Result.Failure("Proposal not found");

            await _unitOfWork.Proposals.DeleteAsync(proposal, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"An error occurred while deleting the proposal: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<ProposalDto>>> SearchProposalsAsync(
        string searchTerm, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var proposals = await _unitOfWork.Proposals.SearchAsync(searchTerm, cancellationToken);
            var proposalDtos = new List<ProposalDto>();
            foreach (var p in proposals)
            {
                var citizen = await _unitOfWork.Citizens.GetByIdAsync(p.CitizenId, cancellationToken);
                proposalDtos.Add(MapToDto(p, citizen?.Name ?? "Unknown"));
            }

            return Result<IEnumerable<ProposalDto>>.Success(proposalDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ProposalDto>>.Failure($"An error occurred while searching proposals: {ex.Message}");
        }
    }

    private static ProposalDto MapToDto(Proposal proposal, string citizenName)
    {
        return new ProposalDto
        {
            Id = proposal.Id,
            Title = proposal.Title,
            Description = proposal.Description,
            Type = proposal.Type,
            Status = proposal.Status,
            Priority = proposal.Priority,
            Location = proposal.Location,
            EstimatedCost = proposal.EstimatedCost,
            TargetCompletionDate = proposal.TargetCompletionDate,
            // Removido: sistema de votos
            CitizenId = proposal.CitizenId,
            CitizenName = citizenName,
            CreatedAt = proposal.CreatedAt,
            UpdatedAt = proposal.UpdatedAt
        };
    }
}
