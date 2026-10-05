using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

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
