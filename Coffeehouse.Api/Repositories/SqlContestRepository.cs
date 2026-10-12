using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// SQL-based implementation of the contest repository using Entity Framework Core.
/// Injected as a Singleton: uses IServiceScopeFactory for safe scope resolution.
/// </summary>
public class SqlContestRepository : IContestRepository
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SqlContestRepository(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<IEnumerable<Contest>> GetAllAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Contests.Include(c => c.Candidates).ToListAsync();
    }

    public async Task<IEnumerable<Contest>> GetContestsByElectionIdAsync(int electionId)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Contests
            .Where(c => c.ElectionId == electionId)
            .Include(c => c.Candidates)
            .ToListAsync();
    }

    public async Task<Contest?> GetByIdAsync(int id)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Contests.Include(c => c.Candidates).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ElectionExistsAsync(int electionId)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Elections.AnyAsync(e => e.Id == electionId);
    }
}
