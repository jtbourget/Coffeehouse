using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// Defines repository operations for retrieving candidate entities.
/// </summary>
public interface ICandidateRepository
{
    Task<IEnumerable<Candidate>> GetAllAsync();
    Task<IEnumerable<Candidate>> GetCandidatesByContestIdAsync(int contestId);
    Task<Candidate?> GetByIdAsync(int id);
    Task<bool> ContestExistsAsync(int contestId);
}
