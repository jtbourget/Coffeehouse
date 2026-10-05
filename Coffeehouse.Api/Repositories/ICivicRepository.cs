using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

public interface ICivicRepository
{
    Task<List<Election>> GetElectionsAsync();
    Task<UserAddress?> GetLatestAddressAsync();
    Task<List<CandidateProfile>> GetCandidateProfilesAsync();
}
