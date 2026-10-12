using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Initializes a new instance of the FavoritesController.
        /// </summary>
        /// <param name="service">The favorites service.</param>
        public FavoritesController(IFavoritesService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all favorites for a specific election.
        /// </summary>
        /// <param name="electionId">The election ID.</param>
        /// <returns>A list of favorite candidates.</returns>
        [HttpGet("api/elections/{electionId}/favorites")]
        public async Task<ActionResult<IEnumerable<FavoriteCandidateResponseDto>>> GetFavoritesForElection(int electionId)
        {
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
            await _service.RemoveFavoriteAsync(contestId);
            return NoContent();
        }
    }
}
