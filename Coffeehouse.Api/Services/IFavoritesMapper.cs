using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Models;
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
