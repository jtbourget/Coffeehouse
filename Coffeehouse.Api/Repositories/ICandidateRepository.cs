using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Repositories;

public interface ICandidateRepository
{
    Task<IEnumerable<Candidate>> GetAllAsync();
    Task<Candidate?> GetByIdAsync(int id);
}
