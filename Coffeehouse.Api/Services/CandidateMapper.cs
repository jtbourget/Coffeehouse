using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Models;

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
            ContestId = entity.ContestId,
            Name = entity.Name ?? string.Empty,
            Party = entity.Party ?? string.Empty,
            PhotoUrl = entity.PhotoUrl ?? string.Empty,
            CampaignWebsite = entity.CampaignWebsite ?? string.Empty,
            Bio = entity.Bio ?? string.Empty
        };
    }
}
