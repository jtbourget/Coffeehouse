using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Repositories;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Scoped service for favorite candidates operations.
/// Depends on IFavoritesRepository.
/// </summary>
public class FavoritesService : IFavoritesService
{
    private readonly IFavoritesRepository _repository;
    private readonly IFavoritesMapper _mapper;

    public FavoritesService(IFavoritesRepository repository, IFavoritesMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FavoriteCandidateResponseDto>> GetFavoritesForElectionAsync(int electionId)
    {
        var favorites = await _repository.GetFavoritesForElectionAsync(electionId);
        return _mapper.ToDtoList(favorites);
    }

    public async Task SetFavoriteAsync(int contestId, int candidateId)
    {
        await _repository.SetFavoriteAsync(contestId, candidateId);
    }

    public async Task RemoveFavoriteAsync(int contestId)
    {
        await _repository.RemoveFavoriteAsync(contestId);
    }
}
