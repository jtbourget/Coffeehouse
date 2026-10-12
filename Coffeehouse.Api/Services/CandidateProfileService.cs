namespace Coffeehouse.Api.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Models;
using Coffeehouse.Api.Repositories;

/// <summary>
/// Scoped business service for candidate profile management.
/// Depends on the ICandidateProfileRepository interface.
/// Enforces business rules against existing data and returns outcomes decoupled from HTTP codes.
/// </summary>
public class CandidateProfileService : ICandidateProfileService
{
    private readonly ICandidateProfileRepository _repository;
    private readonly ICandidateProfileMapper _mapper;

    public CandidateProfileService(ICandidateProfileRepository repository, ICandidateProfileMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CandidateProfileResponseDto>> GetAllProfilesAsync(string? ocdId = null)
    {
        var profiles = await _repository.GetAllProfilesAsync(ocdId);
        return profiles.Select(_mapper.ToDto);
    }

    public async Task<CandidateProfileResponseDto?> GetProfileAsync(int id)
    {
        var profile = await _repository.GetProfileAsync(id);
        return profile == null ? null : _mapper.ToDto(profile);
    }

    public async Task<ServiceOutcome<CandidateProfileResponseDto>> CreateProfileAsync(SaveCandidateProfileRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ServiceOutcome<CandidateProfileResponseDto>.InvalidInput("Candidate name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.OcdId))
        {
            return ServiceOutcome<CandidateProfileResponseDto>.InvalidInput("District identifier (OcdId) is required.");
        }

        // Business rule depending on existing data: Duplicate check
        var existing = await _repository.GetByNameAndOcdIdAsync(request.Name.Trim(), request.OcdId.Trim());
        if (existing != null)
        {
            return ServiceOutcome<CandidateProfileResponseDto>.Duplicate(
                $"A candidate profile already exists for '{request.Name.Trim()}' in district '{request.OcdId.Trim()}'.");
        }

        var entity = _mapper.ToEntity(request);
        // Server-controlled fields: client cannot set these
        entity.CreatedAt = DateTime.UtcNow;
        entity.IsVerified = false;

        var createdProfile = await _repository.CreateProfileAsync(entity);
        return ServiceOutcome<CandidateProfileResponseDto>.Success(_mapper.ToDto(createdProfile));
    }

    public async Task<ServiceOutcome<CandidateProfileResponseDto>> UpdateProfileAsync(int id, SaveCandidateProfileRequestDto request)
    {
        var existing = await _repository.GetProfileAsync(id);
        if (existing == null)
        {
            return ServiceOutcome<CandidateProfileResponseDto>.NotFound($"Candidate profile with ID {id} was not found.");
        }

        // Business rule depending on existing data: Duplicate check against other profiles
        var duplicate = await _repository.GetByNameAndOcdIdAsync(request.Name.Trim(), request.OcdId.Trim());
        if (duplicate != null && duplicate.Id != id)
        {
            return ServiceOutcome<CandidateProfileResponseDto>.Duplicate(
                $"A candidate profile already exists for '{request.Name.Trim()}' in district '{request.OcdId.Trim()}'.");
        }

        var entity = _mapper.ToEntity(request);
        await _repository.UpdateProfileAsync(id, entity);

        var updated = await _repository.GetProfileAsync(id);
        return ServiceOutcome<CandidateProfileResponseDto>.Success(_mapper.ToDto(updated!));
    }

    public async Task<ServiceOutcome<bool>> DeleteProfileAsync(int id)
    {
        var existing = await _repository.GetProfileAsync(id);
        if (existing == null)
        {
            return ServiceOutcome<bool>.NotFound($"Candidate profile with ID {id} was not found.");
        }

        var deleted = await _repository.DeleteProfileAsync(id);
        return ServiceOutcome<bool>.Success(deleted);
    }

    public async Task<ServiceOutcome<VideoResponseDto>> AddVideoAsync(int id, SaveVideoRequestDto request)
    {
        var video = _mapper.ToEntity(request);
        var addedVideo = await _repository.AddVideoAsync(id, video);
        if (addedVideo == null)
        {
            return ServiceOutcome<VideoResponseDto>.NotFound($"Candidate profile with ID {id} was not found.");
        }

        return ServiceOutcome<VideoResponseDto>.Success(_mapper.ToDto(addedVideo));
    }
}
