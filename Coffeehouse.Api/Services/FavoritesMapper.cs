using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Coffeehouse.Api.Services;

public class FavoritesMapper : IFavoritesMapper
{
    public FavoriteCandidateResponseDto ToDto(FavoriteCandidate favorite)
    {
        return new FavoriteCandidateResponseDto
        {
            Id = favorite.Id,
            ContestId = favorite.ContestId,
            OfficeName = favorite.Contest?.OfficeName ?? string.Empty,
            CandidateId = favorite.CandidateId,
            CandidateName = favorite.Candidate?.Name ?? string.Empty,
            Party = favorite.Candidate?.Party ?? string.Empty
        };
    }

    public IEnumerable<FavoriteCandidateResponseDto> ToDtoList(IEnumerable<FavoriteCandidate> favorites)
    {
        return favorites.Select(ToDto).ToList();
    }
}
