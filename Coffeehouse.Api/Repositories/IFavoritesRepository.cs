using Coffeehouse.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coffeehouse.Api.Repositories;

public interface IFavoritesRepository
{
    Task<IEnumerable<FavoriteCandidate>> GetFavoritesForElectionAsync(int electionId);
    Task SetFavoriteAsync(int contestId, int candidateId);
    Task RemoveFavoriteAsync(int contestId);
}
