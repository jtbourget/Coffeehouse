namespace Coffeehouse.Api.Services;

using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Models;

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
