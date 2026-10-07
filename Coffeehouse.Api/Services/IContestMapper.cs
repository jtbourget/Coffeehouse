using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Defines mapping operations between contest entities and data transfer objects.
/// </summary>
public interface IContestMapper
{
    ContestResponseDto ToDto(Contest entity);
}
