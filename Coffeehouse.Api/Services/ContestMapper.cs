using System.Linq;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Provides mapping functionality between contest entities and data transfer objects.
/// </summary>
public class ContestMapper : IContestMapper
{
    private readonly ICandidateMapper _candidateMapper;

    public ContestMapper(ICandidateMapper candidateMapper)
    {
        _candidateMapper = candidateMapper;
    }

    public ContestResponseDto ToDto(Contest entity)
    {
        return new ContestResponseDto
        {
            Id = entity.Id,
            ElectionId = entity.ElectionId,
            OfficeName = entity.OfficeName ?? string.Empty,
            OcdId = entity.OcdId ?? string.Empty,
            Candidates = entity.Candidates?.Select(_candidateMapper.ToDto).ToList() ?? new()
        };
    }
}
