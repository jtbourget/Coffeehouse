using Coffeehouse.Models;

namespace Coffeehouse.Services;

public interface IApiService
{
    Task<UserAddress?> GetAddressAsync();
    Task<bool> SaveAddressAsync(UserAddress address);
    Task<List<Election>> GetElectionsAsync();
    Task<Election?> GetElectionAsync(int id);
    Task<List<Contest>> GetContestsAsync(int electionId);
    Task<List<Candidate>> GetCandidatesAsync(int contestId);
    Task<Candidate?> GetCandidateAsync(int id);
    Task<List<FavoriteCandidate>> GetFavoritesAsync(int electionId);
    Task<bool> SetFavoriteAsync(int contestId, int candidateId);
    Task<bool> RemoveFavoriteAsync(int contestId);
}
