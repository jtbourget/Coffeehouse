namespace Coffeehouse.Api.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;

/// <summary>
/// Service interface defining candidate profile operations.
/// </summary>
public interface ICandidateProfileService
{
    Task<IEnumerable<CandidateProfileResponseDto>> GetAllProfilesAsync(string? ocdId = null);
    Task<CandidateProfileResponseDto?> GetProfileAsync(int id);
    Task<ServiceOutcome<CandidateProfileResponseDto>> CreateProfileAsync(SaveCandidateProfileRequestDto request);
    Task<ServiceOutcome<CandidateProfileResponseDto>> UpdateProfileAsync(int id, SaveCandidateProfileRequestDto request);
    Task<ServiceOutcome<bool>> DeleteProfileAsync(int id);
    Task<ServiceOutcome<VideoResponseDto>> AddVideoAsync(int id, SaveVideoRequestDto request);
}
