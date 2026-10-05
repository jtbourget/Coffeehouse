using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

public interface ICandidateProfileMapper
{
    CandidateProfile ToEntity(SaveCandidateProfileRequestDto dto);
    CandidateProfileResponseDto ToDto(CandidateProfile entity);
    Video ToEntity(SaveVideoRequestDto dto);
    VideoResponseDto ToDto(Video entity);
}
