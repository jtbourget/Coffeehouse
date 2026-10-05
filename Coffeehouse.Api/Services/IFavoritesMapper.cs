using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;
using System.Collections.Generic;

namespace Coffeehouse.Api.Services;

public interface IFavoritesMapper
{
    FavoriteCandidateResponseDto ToDto(FavoriteCandidate favorite);
    IEnumerable<FavoriteCandidateResponseDto> ToDtoList(IEnumerable<FavoriteCandidate> favorites);
}
