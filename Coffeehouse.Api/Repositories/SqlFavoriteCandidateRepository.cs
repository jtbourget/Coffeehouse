using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// SQL-based implementation of the favorite candidate repository using Entity Framework Core.
/// Injected as a Singleton: uses IServiceScopeFactory for safe scope resolution.
/// </summary>
public class SqlFavoriteCandidateRepository : IFavoriteCandidateRepository
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SqlFavoriteCandidateRepository(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<IEnumerable<FavoriteCandidate>> GetAllAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Set<FavoriteCandidate>().ToListAsync();
    }

    public async Task<FavoriteCandidate?> GetByIdAsync(int id)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Set<FavoriteCandidate>().FindAsync(id);
    }
}
