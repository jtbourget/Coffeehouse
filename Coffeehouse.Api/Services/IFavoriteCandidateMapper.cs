using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Defines mapping operations between favorite candidate entities and data transfer objects.
/// </summary>
public interface IFavoriteCandidateMapper
{
    FavoriteCandidateResponseDto ToDto(FavoriteCandidate entity);
}
