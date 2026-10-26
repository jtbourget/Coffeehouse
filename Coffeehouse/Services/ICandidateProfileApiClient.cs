using Coffeehouse.Models;

namespace Coffeehouse.Services;

public interface ICandidateProfileApiClient
{
    Task<ApiResult<List<CandidateProfileDisplay>>> GetProfilesAsync(string? ocdId = null, string? search = null);
    Task<ApiResult<CandidateProfileDisplay>> GetProfileAsync(int id);
    Task<ApiResult<CandidateProfileDisplay>> CreateProfileAsync(SaveCandidateProfileDto dto);
    Task<ApiResult> UpdateProfileAsync(int id, SaveCandidateProfileDto dto);
    Task<ApiResult> DeleteProfileAsync(int id);
}
