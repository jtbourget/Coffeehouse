using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

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
