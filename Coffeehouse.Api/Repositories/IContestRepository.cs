using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

public interface IContestRepository
{
    Task<IEnumerable<Contest>> GetAllAsync();
    Task<Contest?> GetByIdAsync(int id);
}
