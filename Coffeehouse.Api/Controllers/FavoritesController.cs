using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Api.Controllers
{
    /// <summary>
    /// Controller for managing favorite candidates.
    /// Depends on the IFavoritesService interface. Never returns domain models directly.
    /// </summary>
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoritesService _service;
        private readonly ILogger<FavoritesController> _logger;

        /// <summary>
        /// Initializes a new instance of the FavoritesController.
        /// </summary>
        /// <param name="service">The favorites service.</param>
        /// <param name="logger">The logger instance.</param>
        public FavoritesController(IFavoritesService service, ILogger<FavoritesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all favorites for a specific election.
        /// </summary>
        /// <param name="electionId">The election ID.</param>
        /// <returns>A list of favorite candidates.</returns>
        [HttpGet("api/elections/{electionId}/favorites")]
        public async Task<ActionResult<IEnumerable<FavoriteCandidateResponseDto>>> GetFavoritesForElection(int electionId)
        {
            _logger.LogInformation("Handling GET /api/elections/{ElectionId}/favorites request.", electionId);
            var favorites = await _service.GetFavoritesForElectionAsync(electionId);
            return Ok(favorites);
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
            _logger.LogInformation("Handling PUT /api/favorites/{ContestId}/{CandidateId} request.", contestId, candidateId);
            await _service.SetFavoriteAsync(contestId, candidateId);
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
            _logger.LogInformation("Handling DELETE /api/favorites/{ContestId} request.", contestId);
            await _service.RemoveFavoriteAsync(contestId);
            return NoContent();
        }
    }
}
