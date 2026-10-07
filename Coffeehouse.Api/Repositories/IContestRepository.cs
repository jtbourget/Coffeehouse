using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// Defines repository operations for retrieving election contest entities.
/// </summary>
public interface IContestRepository
{
    Task<IEnumerable<Contest>> GetAllAsync();
    Task<Contest?> GetByIdAsync(int id);
}
