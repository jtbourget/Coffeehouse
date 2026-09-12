using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Controllers
{
    /// <summary>
    /// Controller for managing candidates.
    /// </summary>
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the CandidatesController.
        /// </summary>
        /// <param name="context">The database context.</param>
        public CandidatesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves candidates for a specific contest.
        /// </summary>
        /// <param name="contestId">The contest ID.</param>
        /// <returns>A list of candidates.</returns>
        [HttpGet("api/contests/{contestId}/candidates")]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidatesForContest(int contestId)
        {
            var contestExists = await _context.Contests.AnyAsync(c => c.Id == contestId);
            if (!contestExists)
            {
                return NotFound();
            }

            return await _context.Candidates
                .Where(c => c.ContestId == contestId)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single candidate by ID.
        /// </summary>
        /// <param name="id">The candidate ID.</param>
        /// <returns>The candidate if found; otherwise, 404 Not Found.</returns>
        [HttpGet("api/candidates/{id}")]
        public async Task<ActionResult<Candidate>> GetCandidate(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);

            if (candidate == null)
            {
                return NotFound();
            }

            return candidate;
        }
    }
}
