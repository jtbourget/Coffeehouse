using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Provides mapping functionality between candidate entities and data transfer objects.
/// </summary>
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
