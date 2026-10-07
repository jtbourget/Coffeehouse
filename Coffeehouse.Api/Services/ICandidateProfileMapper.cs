using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Defines mapping operations between candidate profile and video entities and their corresponding data transfer objects.
/// </summary>
public interface ICandidateProfileMapper
{
    CandidateProfile ToEntity(SaveCandidateProfileRequestDto dto);
    CandidateProfileResponseDto ToDto(CandidateProfile entity);
    Video ToEntity(SaveVideoRequestDto dto);
    VideoResponseDto ToDto(Video entity);
}
