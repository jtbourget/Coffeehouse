using Coffeehouse.Api.Models.DTOs;
using Coffeehouse.Api.Repositories;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Coffeehouse.Api.Controllers
{
    /// <summary>
    /// Controller for managing favorite candidates.
    /// </summary>
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoritesRepository _repository;
        private readonly IFavoritesMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the FavoritesController.
        /// </summary>
        /// <param name="repository">The favorites repository.</param>
        /// <param name="mapper">The favorites mapper.</param>
        public FavoritesController(IFavoritesRepository repository, IFavoritesMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all favorites for a specific election.
        /// </summary>
        /// <param name="electionId">The election ID.</param>
        /// <returns>A list of favorite candidates.</returns>
        [HttpGet("api/elections/{electionId}/favorites")]
        public async Task<ActionResult<IEnumerable<FavoriteCandidateResponseDto>>> GetFavoritesForElection(int electionId)
        {
            var favorites = await _repository.GetFavoritesForElectionAsync(electionId);
            return Ok(_mapper.ToDtoList(favorites));
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
            await _repository.SetFavoriteAsync(contestId, candidateId);
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
            await _repository.RemoveFavoriteAsync(contestId);
            return NoContent();
        }
    }
}
