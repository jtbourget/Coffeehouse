using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Api.Controllers
{
    /// <summary>
    /// Controller for managing candidates.
    /// Depends on the ICandidateService interface. Never returns the Model.
    /// </summary>
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateService _service;
        private readonly ILogger<CandidatesController> _logger;

        /// <summary>
        /// Initializes a new instance of the CandidatesController.
        /// </summary>
        /// <param name="service">The candidate service.</param>
        /// <param name="logger">The logger instance.</param>
        public CandidatesController(ICandidateService service, ILogger<CandidatesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves candidates for a specific contest.
        /// </summary>
        /// <param name="contestId">The contest ID.</param>
        /// <returns>A list of candidate response DTOs.</returns>
        [HttpGet("api/contests/{contestId}/candidates")]
        public async Task<ActionResult<IEnumerable<CandidateResponseDto>>> GetCandidatesForContest(int contestId)
        {
            _logger.LogInformation("Handling GET /api/contests/{ContestId}/candidates request.", contestId);
            var candidates = await _service.GetCandidatesForContestAsync(contestId);
            if (candidates == null)
            {
                return NotFound();
            }

            return Ok(candidates);
        }

        /// <summary>
        /// Retrieves a single candidate by ID.
        /// </summary>
        /// <param name="id">The candidate ID.</param>
        /// <returns>The candidate response DTO if found; otherwise, 404 Not Found.</returns>
        [HttpGet("api/candidates/{id}")]
        public async Task<ActionResult<CandidateResponseDto>> GetCandidate(int id)
        {
            _logger.LogInformation("Handling GET /api/candidates/{Id} request.", id);
            var candidate = await _service.GetCandidateAsync(id);

            if (candidate == null)
            {
                return NotFound();
            }

            return Ok(candidate);
        }
    }
}
