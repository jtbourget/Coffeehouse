using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Controllers
{
    /// <summary>
    /// Controller for managing elections.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ElectionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the ElectionsController.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ElectionsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all elections including their contests.
        /// </summary>
        /// <returns>A list of elections.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Election>>> GetElections()
        {
            // Retrieve all elections and eager load their associated contests
            return await _context.Elections
                .Include(e => e.Contests)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single election by ID, including its contests and candidates.
        /// </summary>
        /// <param name="id">The election ID.</param>
        /// <returns>The election if found; otherwise, 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Election>> GetElection(int id)
        {
            // Retrieve the specific election by ID, eager loading both its contests and the candidates within each contest
            var election = await _context.Elections
                .Include(e => e.Contests)
                    .ThenInclude(c => c.Candidates)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (election == null)
            {
                return NotFound();
            }

            return election;
        }
    }
}
