using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;
using System.Collections.Generic;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Defines mapping operations between favorite candidate entities and candidate response data transfer objects.
/// </summary>
public interface IFavoritesMapper
{
    FavoriteCandidateResponseDto ToDto(FavoriteCandidate favorite);
    IEnumerable<FavoriteCandidateResponseDto> ToDtoList(IEnumerable<FavoriteCandidate> favorites);
}
