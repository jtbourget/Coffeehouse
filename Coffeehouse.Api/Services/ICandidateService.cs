using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Service interface for candidate retrieval operations.
/// </summary>
public interface ICandidateService
{
    Task<IEnumerable<CandidateResponseDto>?> GetCandidatesForContestAsync(int contestId);
    Task<CandidateResponseDto?> GetCandidateAsync(int id);
}
