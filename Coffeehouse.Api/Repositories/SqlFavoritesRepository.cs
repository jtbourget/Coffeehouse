using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coffeehouse.Api.Repositories;

public class SqlFavoritesRepository : IFavoritesRepository
{
    private readonly AppDbContext _context;

    public SqlFavoritesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FavoriteCandidate>> GetFavoritesForElectionAsync(int electionId)
    {
        return await _context.FavoriteCandidates
            .Include(f => f.Contest)
            .Include(f => f.Candidate)
            .Where(f => f.Contest != null && f.Contest.ElectionId == electionId)
            .ToListAsync();
    }

    public async Task SetFavoriteAsync(int contestId, int candidateId)
    {
        var favorite = await _context.FavoriteCandidates
            .FirstOrDefaultAsync(f => f.ContestId == contestId);

        if (favorite == null)
        {
            favorite = new FavoriteCandidate
            {
                ContestId = contestId,
                CandidateId = candidateId
            };
            _context.FavoriteCandidates.Add(favorite);
        }
        else
        {
            favorite.CandidateId = candidateId;
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveFavoriteAsync(int contestId)
    {
        var favorite = await _context.FavoriteCandidates
            .FirstOrDefaultAsync(f => f.ContestId == contestId);

        if (favorite != null)
        {
            _context.FavoriteCandidates.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }
}
