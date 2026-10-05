using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

public interface IContestMapper
{
    ContestResponseDto ToDto(Contest entity);
}
