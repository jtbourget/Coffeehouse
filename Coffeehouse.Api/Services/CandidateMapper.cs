using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

public class CandidateMapper : ICandidateMapper
{
    public CandidateResponseDto ToDto(Candidate entity)
    {
        return new CandidateResponseDto
        {
            Id = entity.Id,
        };
    }
}
