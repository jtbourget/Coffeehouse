using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// SQL-based implementation of the candidate repository using Entity Framework Core.
/// </summary>
public class SqlCandidateRepository : ICandidateRepository
{
    private readonly AppDbContext _context;

    public SqlCandidateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Candidate>> GetAllAsync()
    {
        return await _context.Set<Candidate>().ToListAsync();
    }

    public async Task<Candidate?> GetByIdAsync(int id)
    {
        return await _context.Set<Candidate>().FindAsync(id);
    }
}
