using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Defines mapping operations between candidate entities and data transfer objects.
/// </summary>
public interface ICandidateMapper
{
    CandidateResponseDto ToDto(Candidate entity);
}
