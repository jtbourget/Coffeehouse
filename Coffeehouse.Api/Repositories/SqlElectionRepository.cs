using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Repositories;

public class SqlElectionRepository : IElectionRepository
{
    private readonly AppDbContext _context;

    public SqlElectionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Election>> GetAllAsync()
    {
        return await _context.Set<Election>().ToListAsync();
    }

    public async Task<Election?> GetByIdAsync(int id)
    {
        return await _context.Set<Election>().FindAsync(id);
    }
}
