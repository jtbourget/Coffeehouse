namespace Coffeehouse.Api.Repositories;

using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.Models;

/// <summary>
/// Defines repository operations for retrieving, creating, and updating candidate profiles.
/// </summary>
public interface ICandidateProfileRepository
{
    Task<IEnumerable<CandidateProfile>> GetAllProfilesAsync(string? ocdId = null);
    Task<CandidateProfile?> GetProfileAsync(int id);
    Task<CandidateProfile?> GetByNameAndOcdIdAsync(string name, string ocdId);
    Task<CandidateProfile> CreateProfileAsync(CandidateProfile profile);
    Task<bool> UpdateProfileAsync(int id, CandidateProfile profile);
    Task<bool> DeleteProfileAsync(int id);
    Task<Video?> AddVideoAsync(int candidateProfileId, Video video);
    Task<bool> ProfileExistsAsync(int id);
}
