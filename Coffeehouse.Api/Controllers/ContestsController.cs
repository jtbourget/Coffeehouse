using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Coffeehouse.Api.Controllers
{
    /// <summary>
    /// Controller for managing contests.
    /// Depends on the IContestService interface. Never returns the Model.
    /// </summary>
    [ApiController]
    public class ContestsController : ControllerBase
    {
        private readonly IContestService _service;

        /// <summary>
        /// Initializes a new instance of the ContestsController.
        /// </summary>
        /// <param name="service">The contest service.</param>
        public ContestsController(IContestService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves contests for a specific election, including their candidates.
        /// </summary>
        /// <param name="electionId">The election ID.</param>
        /// <returns>A list of contest response DTOs.</returns>
        [HttpGet("api/elections/{electionId}/contests")]
        public async Task<ActionResult<IEnumerable<ContestResponseDto>>> GetContestsForElection(int electionId)
        {
            var contests = await _service.GetContestsForElectionAsync(electionId);
            if (contests == null)
            {
                return NotFound();
            }

            return Ok(contests);
        }
    }
}
