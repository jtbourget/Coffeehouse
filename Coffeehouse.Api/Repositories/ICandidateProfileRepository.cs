using System.Threading.Tasks;
using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

public interface ICandidateProfileRepository
{
    Task<CandidateProfile?> GetProfileAsync(int id);
    Task<CandidateProfile> CreateProfileAsync(CandidateProfile profile);
    Task UpdateProfileAsync(CandidateProfile profile);
    Task<Video> AddVideoAsync(int candidateProfileId, Video video);
    Task<bool> ProfileExistsAsync(int id);
}
