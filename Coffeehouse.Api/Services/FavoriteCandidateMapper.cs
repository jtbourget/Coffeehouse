using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Provides mapping functionality between favorite candidate entities and data transfer objects.
/// </summary>
public class FavoriteCandidateMapper : IFavoriteCandidateMapper
{
    public FavoriteCandidateResponseDto ToDto(FavoriteCandidate entity)
    {
        return new FavoriteCandidateResponseDto
        {
            Id = entity.Id,
        };
    }
}
