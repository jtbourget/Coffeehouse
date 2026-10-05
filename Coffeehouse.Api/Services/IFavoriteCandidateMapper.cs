using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

public interface IFavoriteCandidateMapper
{
    FavoriteCandidateResponseDto ToDto(FavoriteCandidate entity);
}
