using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Defines mapping operations between candidate entities and data transfer objects.
/// </summary>
public interface ICandidateMapper
{
    CandidateResponseDto ToDto(Candidate entity);
}
