using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

public interface IFavoriteCandidateRepository
{
    Task<IEnumerable<FavoriteCandidate>> GetAllAsync();
    Task<FavoriteCandidate?> GetByIdAsync(int id);
}
