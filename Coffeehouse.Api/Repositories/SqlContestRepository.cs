using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Repositories;

public class SqlContestRepository : IContestRepository
{
    private readonly AppDbContext _context;

    public SqlContestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contest>> GetAllAsync()
    {
        return await _context.Set<Contest>().ToListAsync();
    }

    public async Task<Contest?> GetByIdAsync(int id)
    {
        return await _context.Set<Contest>().FindAsync(id);
    }
}
