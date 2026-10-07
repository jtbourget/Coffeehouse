using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Repositories;

/// <summary>
/// SQL-based implementation of the favorite candidate repository using Entity Framework Core.
/// </summary>
public class SqlFavoriteCandidateRepository : IFavoriteCandidateRepository
{
    private readonly AppDbContext _context;

    public SqlFavoriteCandidateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FavoriteCandidate>> GetAllAsync()
    {
        return await _context.Set<FavoriteCandidate>().ToListAsync();
    }

    public async Task<FavoriteCandidate?> GetByIdAsync(int id)
    {
        return await _context.Set<FavoriteCandidate>().FindAsync(id);
    }
}
