using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// SQL-based implementation of the favorites repository using Entity Framework Core.
/// Injected as a Singleton: uses IServiceScopeFactory for safe scope resolution.
/// </summary>
public class SqlFavoritesRepository : IFavoritesRepository
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SqlFavoritesRepository(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<IEnumerable<FavoriteCandidate>> GetFavoritesForElectionAsync(int electionId)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.FavoriteCandidates
            .Include(f => f.Contest)
            .Include(f => f.Candidate)
            .Where(f => f.Contest != null && f.Contest.ElectionId == electionId)
            .ToListAsync();
    }

    public async Task SetFavoriteAsync(int contestId, int candidateId)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var favorite = await context.FavoriteCandidates
            .FirstOrDefaultAsync(f => f.ContestId == contestId);

        if (favorite == null)
        {
            favorite = new FavoriteCandidate
            {
                ContestId = contestId,
                CandidateId = candidateId
            };
            context.FavoriteCandidates.Add(favorite);
        }
        else
        {
            favorite.CandidateId = candidateId;
        }

        await context.SaveChangesAsync();
    }

    public async Task RemoveFavoriteAsync(int contestId)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var favorite = await context.FavoriteCandidates
            .FirstOrDefaultAsync(f => f.ContestId == contestId);

        if (favorite != null)
        {
            context.FavoriteCandidates.Remove(favorite);
            await context.SaveChangesAsync();
        }
    }
}
