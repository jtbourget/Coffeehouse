using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// SQL-based implementation of the candidate repository using Entity Framework Core.
/// Injected as a Singleton: uses IServiceScopeFactory for safe scope resolution.
/// </summary>
public class SqlCandidateRepository : ICandidateRepository
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SqlCandidateRepository(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<IEnumerable<Candidate>> GetAllAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Candidates.ToListAsync();
    }

    public async Task<IEnumerable<Candidate>> GetCandidatesByContestIdAsync(int contestId)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Candidates
            .Where(c => c.ContestId == contestId)
            .ToListAsync();
    }

    public async Task<Candidate?> GetByIdAsync(int id)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Candidates.FindAsync(id);
    }

    public async Task<bool> ContestExistsAsync(int contestId)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Contests.AnyAsync(c => c.Id == contestId);
    }
}
