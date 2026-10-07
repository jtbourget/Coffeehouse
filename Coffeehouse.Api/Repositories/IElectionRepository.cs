using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// Defines repository operations for retrieving election entities.
/// </summary>
public interface IElectionRepository
{
    Task<IEnumerable<Election>> GetAllAsync();
    Task<Election?> GetByIdAsync(int id);
}
