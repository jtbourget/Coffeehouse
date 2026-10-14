using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Repositories;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Scoped service for favorite candidates operations.
/// Depends on IFavoritesRepository.
/// </summary>
public class FavoritesService : IFavoritesService
{
    private readonly IFavoritesRepository _repository;
    private readonly IFavoritesMapper _mapper;
    private readonly ILogger<FavoritesService> _logger;

    public FavoritesService(IFavoritesRepository repository, IFavoritesMapper mapper, ILogger<FavoritesService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<FavoriteCandidateResponseDto>> GetFavoritesForElectionAsync(int electionId)
    {
        _logger.LogDebug("Retrieving favorites for election ID {ElectionId}.", electionId);
        var favorites = await _repository.GetFavoritesForElectionAsync(electionId);
        return _mapper.ToDtoList(favorites);
    }

    public async Task SetFavoriteAsync(int contestId, int candidateId)
    {
        _logger.LogInformation("Setting favorite candidate {CandidateId} for contest {ContestId}.", candidateId, contestId);
        await _repository.SetFavoriteAsync(contestId, candidateId);
    }

    public async Task RemoveFavoriteAsync(int contestId)
    {
        _logger.LogInformation("Removing favorite candidate for contest {ContestId}.", contestId);
        await _repository.RemoveFavoriteAsync(contestId);
    }
}
