using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Defines mapping operations between favorite candidate entities and data transfer objects.
/// </summary>
public interface IFavoriteCandidateMapper
{
    FavoriteCandidateResponseDto ToDto(FavoriteCandidate entity);
}
