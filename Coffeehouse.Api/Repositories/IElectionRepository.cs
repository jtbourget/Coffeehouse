using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

public interface IElectionRepository
{
    Task<IEnumerable<Election>> GetAllAsync();
    Task<Election?> GetByIdAsync(int id);
}
