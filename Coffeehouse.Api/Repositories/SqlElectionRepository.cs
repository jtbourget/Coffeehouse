using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// SQL-based implementation of the election repository using Entity Framework Core.
/// Injected as a Singleton: uses IServiceScopeFactory for safe scope resolution.
/// </summary>
public class SqlElectionRepository : IElectionRepository
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SqlElectionRepository(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<IEnumerable<Election>> GetAllAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Elections
            .Include(e => e.Contests)
                .ThenInclude(c => c.Candidates)
            .ToListAsync();
    }

    public async Task<Election?> GetByIdAsync(int id)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Elections
            .Include(e => e.Contests)
                .ThenInclude(c => c.Candidates)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}
