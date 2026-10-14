namespace Coffeehouse.Api.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Models;
using Coffeehouse.Api.Repositories;
using Microsoft.Extensions.Logging;

/// <summary>
/// Scoped business service for candidate profile management.
/// Depends on the ICandidateProfileRepository interface.
/// Enforces business rules against existing data and returns outcomes decoupled from HTTP codes.
/// </summary>
public class CandidateProfileService : ICandidateProfileService
{
    private readonly ICandidateProfileRepository _repository;
    private readonly ICandidateProfileMapper _mapper;
    private readonly ILogger<CandidateProfileService> _logger;

    public CandidateProfileService(
        ICandidateProfileRepository repository,
        ICandidateProfileMapper mapper,
        ILogger<CandidateProfileService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<CandidateProfileResponseDto>> GetAllProfilesAsync(string? ocdId = null)
    {
        _logger.LogDebug("Retrieving candidate profiles with filter ocdId: {OcdId}", ocdId ?? "(none)");
        var profiles = await _repository.GetAllProfilesAsync(ocdId);
        return profiles.Select(_mapper.ToDto);
    }

    public async Task<CandidateProfileResponseDto?> GetProfileAsync(int id)
    {
        _logger.LogDebug("Retrieving candidate profile ID {Id}", id);
        var profile = await _repository.GetProfileAsync(id);
        if (profile == null)
        {
            _logger.LogInformation("Candidate profile ID {Id} was not found.", id);
            return null;
        }

        return _mapper.ToDto(profile);
    }

    public async Task<ServiceOutcome<CandidateProfileResponseDto>> CreateProfileAsync(SaveCandidateProfileRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            _logger.LogWarning("CreateProfile failed: Candidate name is required.");
            return ServiceOutcome<CandidateProfileResponseDto>.InvalidInput("Candidate name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.OcdId))
        {
            _logger.LogWarning("CreateProfile failed: District identifier (OcdId) is required.");
            return ServiceOutcome<CandidateProfileResponseDto>.InvalidInput("District identifier (OcdId) is required.");
        }

        // Business rule depending on existing data: Duplicate check
        var existing = await _repository.GetByNameAndOcdIdAsync(request.Name.Trim(), request.OcdId.Trim());
        if (existing != null)
        {
            _logger.LogWarning("CreateProfile failed: Candidate profile already exists for '{Name}' in district '{OcdId}'.", request.Name.Trim(), request.OcdId.Trim());
            return ServiceOutcome<CandidateProfileResponseDto>.Duplicate(
                $"A candidate profile already exists for '{request.Name.Trim()}' in district '{request.OcdId.Trim()}'.");
        }

        var entity = _mapper.ToEntity(request);
        // Server-controlled fields: client cannot set these
        entity.CreatedAt = DateTime.UtcNow;
        entity.IsVerified = false;

        var createdProfile = await _repository.CreateProfileAsync(entity);
        _logger.LogInformation("Created candidate profile with ID {Id} for '{Name}'.", createdProfile.Id, createdProfile.Name);
        return ServiceOutcome<CandidateProfileResponseDto>.Success(_mapper.ToDto(createdProfile));
    }

    public async Task<ServiceOutcome<CandidateProfileResponseDto>> UpdateProfileAsync(int id, SaveCandidateProfileRequestDto request)
    {
        var existing = await _repository.GetProfileAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("UpdateProfile failed: Candidate profile with ID {Id} was not found.", id);
            return ServiceOutcome<CandidateProfileResponseDto>.NotFound($"Candidate profile with ID {id} was not found.");
        }

        // Business rule depending on existing data: Duplicate check against other profiles
        var duplicate = await _repository.GetByNameAndOcdIdAsync(request.Name.Trim(), request.OcdId.Trim());
        if (duplicate != null && duplicate.Id != id)
        {
            _logger.LogWarning("UpdateProfile failed: Candidate profile already exists for '{Name}' in district '{OcdId}' (ID {ExistingId}).", request.Name.Trim(), request.OcdId.Trim(), duplicate.Id);
            return ServiceOutcome<CandidateProfileResponseDto>.Duplicate(
                $"A candidate profile already exists for '{request.Name.Trim()}' in district '{request.OcdId.Trim()}'.");
        }

        var entity = _mapper.ToEntity(request);
        await _repository.UpdateProfileAsync(id, entity);

        var updated = await _repository.GetProfileAsync(id);
        _logger.LogInformation("Updated candidate profile with ID {Id}.", id);
        return ServiceOutcome<CandidateProfileResponseDto>.Success(_mapper.ToDto(updated!));
    }

    public async Task<ServiceOutcome<bool>> DeleteProfileAsync(int id)
    {
        var existing = await _repository.GetProfileAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("DeleteProfile failed: Candidate profile with ID {Id} was not found.", id);
            return ServiceOutcome<bool>.NotFound($"Candidate profile with ID {id} was not found.");
        }

        var deleted = await _repository.DeleteProfileAsync(id);
        _logger.LogInformation("Deleted candidate profile with ID {Id}.", id);
        return ServiceOutcome<bool>.Success(deleted);
    }

    public async Task<ServiceOutcome<VideoResponseDto>> AddVideoAsync(int id, SaveVideoRequestDto request)
    {
        var video = _mapper.ToEntity(request);
        var addedVideo = await _repository.AddVideoAsync(id, video);
        if (addedVideo == null)
        {
            _logger.LogWarning("AddVideo failed: Candidate profile with ID {Id} was not found.", id);
            return ServiceOutcome<VideoResponseDto>.NotFound($"Candidate profile with ID {id} was not found.");
        }

        _logger.LogInformation("Added video {VideoId} to candidate profile with ID {Id}.", addedVideo.Id, id);
        return ServiceOutcome<VideoResponseDto>.Success(_mapper.ToDto(addedVideo));
    }
}
