using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Controllers
{
    /// <summary>
    /// Controller for managing favorite candidates.
    /// </summary>
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the FavoritesController.
        /// </summary>
        /// <param name="context">The database context.</param>
        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all favorites for a specific election.
        /// </summary>
        /// <param name="electionId">The election ID.</param>
        /// <returns>A list of favorite candidates.</returns>
        [HttpGet("api/elections/{electionId}/favorites")]
        public async Task<ActionResult<IEnumerable<FavoriteCandidate>>> GetFavoritesForElection(int electionId)
        {
            // Retrieve all favorite candidates for the given election by checking the related contest's ElectionId
            // Eager load both the Contest and Candidate details for the response
            return await _context.FavoriteCandidates
                .Include(f => f.Contest)
                .Include(f => f.Candidate)
                .Where(f => f.Contest != null && f.Contest.ElectionId == electionId)
                .ToListAsync();
        }

        /// <summary>
        /// Sets or replaces a favorite candidate for a contest.
        /// </summary>
        /// <param name="contestId">The contest ID.</param>
        /// <param name="candidateId">The candidate ID.</param>
        /// <returns>No content on success.</returns>
        [HttpPut("api/favorites/{contestId}/{candidateId}")]
        public async Task<IActionResult> SetFavorite(int contestId, int candidateId)
        {
            // Search for an existing favorite entry for the specified contest
            var favorite = await _context.FavoriteCandidates
                .FirstOrDefaultAsync(f => f.ContestId == contestId);

            if (favorite == null)
            {
                // If the user hasn't favorited a candidate for this contest yet, create a new entry
                favorite = new FavoriteCandidate
                {
                    ContestId = contestId,
                    CandidateId = candidateId
                };
                _context.FavoriteCandidates.Add(favorite);
            }
            else
            {
                // If an entry already exists, update it to the newly selected candidate
                favorite.CandidateId = candidateId;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Removes a favorite candidate for a contest.
        /// </summary>
        /// <param name="contestId">The contest ID.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("api/favorites/{contestId}")]
        public async Task<IActionResult> RemoveFavorite(int contestId)
        {
            // Search for the existing favorite entry for the specified contest
            var favorite = await _context.FavoriteCandidates
                .FirstOrDefaultAsync(f => f.ContestId == contestId);

            if (favorite != null)
            {
                // If found, remove the favorite entry from the database
                _context.FavoriteCandidates.Remove(favorite);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}
