using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// Defines repository operations for retrieving favorite candidate entities.
/// </summary>
public interface IFavoriteCandidateRepository
{
    Task<IEnumerable<FavoriteCandidate>> GetAllAsync();
    Task<FavoriteCandidate?> GetByIdAsync(int id);
}
