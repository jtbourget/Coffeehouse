using System.Linq;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Provides mapping functionality between election entities and data transfer objects.
/// </summary>
public class ElectionMapper : IElectionMapper
{
    private readonly IContestMapper _contestMapper;

    public ElectionMapper(IContestMapper contestMapper)
    {
        _contestMapper = contestMapper;
    }

    public ElectionResponseDto ToDto(Election entity)
    {
        return new ElectionResponseDto
        {
            Id = entity.Id,
            Name = entity.Name ?? string.Empty,
            ElectionDate = entity.ElectionDate,
            VotingLocationName = entity.VotingLocationName ?? string.Empty,
            VotingLocationAddress = entity.VotingLocationAddress ?? string.Empty,
            Contests = entity.Contests?.Select(_contestMapper.ToDto).ToList() ?? new()
        };
    }
}
