using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;

namespace Coffeehouse.Api.Services;

/// <summary>
/// Service interface for contest operations.
/// </summary>
public interface IContestService
{
    Task<IEnumerable<ContestResponseDto>?> GetContestsForElectionAsync(int electionId);
}
