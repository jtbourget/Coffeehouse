using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

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
