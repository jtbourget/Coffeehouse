using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Provides mapping functionality between contest entities and data transfer objects.
/// </summary>
public class ContestMapper : IContestMapper
{
    public ContestResponseDto ToDto(Contest entity)
    {
        return new ContestResponseDto
        {
            Id = entity.Id,
        };
    }
}
