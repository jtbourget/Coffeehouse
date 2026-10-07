using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Provides mapping functionality between election entities and data transfer objects.
/// </summary>
public class ElectionMapper : IElectionMapper
{
    public ElectionResponseDto ToDto(Election entity)
    {
        return new ElectionResponseDto
        {
            Id = entity.Id,
        };
    }
}
