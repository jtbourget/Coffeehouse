using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Service interface for favorite candidates operations.
/// </summary>
public interface IFavoritesService
{
    Task<IEnumerable<FavoriteCandidateResponseDto>> GetFavoritesForElectionAsync(int electionId);
    Task SetFavoriteAsync(int contestId, int candidateId);
    Task RemoveFavoriteAsync(int contestId);
}
