using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Controllers
{
    /// <summary>
    /// Controller for managing contests.
    /// </summary>
    [ApiController]
    public class ContestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the ContestsController.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ContestsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves contests for a specific election, including their candidates.
        /// </summary>
        /// <param name="electionId">The election ID.</param>
        /// <returns>A list of contests.</returns>
        [HttpGet("api/elections/{electionId}/contests")]
        public async Task<ActionResult<IEnumerable<Contest>>> GetContestsForElection(int electionId)
        {
            var electionExists = await _context.Elections.AnyAsync(e => e.Id == electionId);
            if (!electionExists)
            {
                return NotFound();
            }

            return await _context.Contests
                .Where(c => c.ElectionId == electionId)
                .Include(c => c.Candidates)
                .ToListAsync();
        }
    }
}
