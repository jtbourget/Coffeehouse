using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

public class JsonCandidateProfileRepository : ICandidateProfileRepository
{
    private readonly JsonStoreState _state;
    private readonly ILogger<JsonCandidateProfileRepository> _logger;

    public JsonCandidateProfileRepository(JsonStoreState state, ILogger<JsonCandidateProfileRepository> logger)
    {
        _state = state;
        _logger = logger;
    }

    public Task<IEnumerable<CandidateProfile>> GetAllProfilesAsync(string? ocdId = null)
    {
        var profiles = _state.Snapshot();
        if (!string.IsNullOrWhiteSpace(ocdId))
        {
            var trimmedOcdId = ocdId.Trim().ToLowerInvariant();
            profiles = profiles.Where(p => p.OcdId.ToLowerInvariant() == trimmedOcdId).ToList();
        }

        return Task.FromResult<IEnumerable<CandidateProfile>>(profiles);
    }

    public Task<CandidateProfile?> GetProfileAsync(int id)
    {
        return Task.FromResult(_state.Find(id));
    }

    public Task<CandidateProfile?> GetByNameAndOcdIdAsync(string name, string ocdId)
    {
        return Task.FromResult(_state.FindByNameAndOcdId(name, ocdId));
    }

    public Task<CandidateProfile> CreateProfileAsync(CandidateProfile profile)
    {
        var created = _state.Add(profile);
        _logger.LogInformation("JSON Store: Created candidate profile {Id} ('{Name}')", created.Id, created.Name);
        return Task.FromResult(created);
    }

    public Task<bool> UpdateProfileAsync(int id, CandidateProfile profile)
    {
        var success = _state.Update(id, profile);
        if (success)
        {
            _logger.LogInformation("JSON Store: Updated candidate profile {Id}", id);
        }
        return Task.FromResult(success);
    }

    public Task<bool> DeleteProfileAsync(int id)
    {
        var success = _state.Remove(id);
        if (success)
        {
            _logger.LogInformation("JSON Store: Deleted candidate profile {Id}", id);
        }
        return Task.FromResult(success);
    }

    public Task<Video?> AddVideoAsync(int candidateProfileId, Video video)
    {
        return Task.FromResult(_state.AddVideo(candidateProfileId, video));
    }

    public Task<bool> ProfileExistsAsync(int id)
    {
        return Task.FromResult(_state.Exists(id));
    }
}
