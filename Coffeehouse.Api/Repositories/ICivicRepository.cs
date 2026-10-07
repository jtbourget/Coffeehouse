using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// Defines repository operations for retrieving civic data including elections, addresses, and candidate profiles.
/// </summary>
public interface ICivicRepository
{
    Task<List<Election>> GetElectionsAsync();
    Task<UserAddress?> GetLatestAddressAsync();
    Task<List<CandidateProfile>> GetCandidateProfilesAsync();
}
