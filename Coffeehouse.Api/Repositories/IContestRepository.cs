using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// Defines repository operations for retrieving election contest entities.
/// </summary>
public interface IContestRepository
{
    Task<IEnumerable<Contest>> GetAllAsync();
    Task<IEnumerable<Contest>> GetContestsByElectionIdAsync(int electionId);
    Task<Contest?> GetByIdAsync(int id);
    Task<bool> ElectionExistsAsync(int electionId);
}
