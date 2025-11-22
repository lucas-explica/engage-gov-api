using EngageGov.Application.DTOs.Citizens;
using EngageGov.Application.DTOs.Common;
using EngageGov.Application.Interfaces;
using EngageGov.Domain.Entities;
using EngageGov.Domain.Interfaces;

namespace EngageGov.Application.Services;

/// <summary>
/// Service implementation for citizen operations
/// Follows Single Responsibility Principle
/// </summary>
public class CitizenService : ICitizenService
{
    private readonly IUnitOfWork _unitOfWork;

    public CitizenService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<CitizenDto>> CreateCitizenAsync(
        CreateCitizenDto dto, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if email already exists
            if (await _unitOfWork.Citizens.EmailExistsAsync(dto.Email, cancellationToken))
                return Result<CitizenDto>.Failure("A citizen with this email already exists");

            // Check if document number already exists
            if (await _unitOfWork.Citizens.DocumentNumberExistsAsync(dto.DocumentNumber, cancellationToken))
                return Result<CitizenDto>.Failure("A citizen with this document number already exists");

            // Create citizen entity
            var citizen = new Citizen(
                dto.FullName,
                dto.Email,
                dto.DocumentNumber,
                dto.PhoneNumber
            );

            // Save to repository
            await _unitOfWork.Citizens.AddAsync(citizen, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Map to DTO
            var citizenDto = MapToDto(citizen);

            return Result<CitizenDto>.Success(citizenDto);
        }
        catch (ArgumentException ex)
        {
            return Result<CitizenDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result<CitizenDto>.Failure($"An error occurred while creating the citizen: {ex.Message}");
        }
    }

    public async Task<Result<CitizenDto>> GetCitizenByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var citizen = await _unitOfWork.Citizens.GetByIdAsync(id, cancellationToken);
            if (citizen == null)
                return Result<CitizenDto>.Failure("Citizen not found");

            var citizenDto = MapToDto(citizen);
            return Result<CitizenDto>.Success(citizenDto);
        }
        catch (Exception ex)
        {
            return Result<CitizenDto>.Failure($"An error occurred while retrieving the citizen: {ex.Message}");
        }
    }

    public async Task<Result<CitizenDto>> GetCitizenByEmailAsync(
        string email, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var citizen = await _unitOfWork.Citizens.GetByEmailAsync(email, cancellationToken);
            if (citizen == null)
                return Result<CitizenDto>.Failure("Citizen not found");

            var citizenDto = MapToDto(citizen);
            return Result<CitizenDto>.Success(citizenDto);
        }
        catch (Exception ex)
        {
            return Result<CitizenDto>.Failure($"An error occurred while retrieving the citizen: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<CitizenDto>>> GetAllCitizensAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var citizens = await _unitOfWork.Citizens.GetAllAsync(cancellationToken);
            var citizenDtos = citizens.Select(MapToDto);

            return Result<IEnumerable<CitizenDto>>.Success(citizenDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<CitizenDto>>.Failure($"An error occurred while retrieving citizens: {ex.Message}");
        }
    }

    public async Task<Result> DeleteCitizenAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var citizen = await _unitOfWork.Citizens.GetByIdAsync(id, cancellationToken);
            if (citizen == null)
                return Result.Failure("Citizen not found");

            await _unitOfWork.Citizens.DeleteAsync(citizen, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"An error occurred while deleting the citizen: {ex.Message}");
        }
    }

    private static CitizenDto MapToDto(Citizen citizen)
    {
        return new CitizenDto
        {
            Id = citizen.Id,
            FullName = citizen.FullName,
            Email = citizen.Email,
            PhoneNumber = citizen.PhoneNumber,
            IsEmailVerified = citizen.IsEmailVerified,
            IsActive = citizen.IsActive,
            CreatedAt = citizen.CreatedAt
        };
    }
}
