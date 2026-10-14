using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Api.Controllers
{
    /// <summary>
    /// Controller for managing elections.
    /// Depends on the IElectionService interface. Never returns the Model.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ElectionsController : ControllerBase
    {
        private readonly IElectionService _service;
        private readonly ILogger<ElectionsController> _logger;

        /// <summary>
        /// Initializes a new instance of the ElectionsController.
        /// </summary>
        /// <param name="service">The election service.</param>
        /// <param name="logger">The logger instance.</param>
        public ElectionsController(IElectionService service, ILogger<ElectionsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all elections including their contests.
        /// </summary>
        /// <returns>A list of election response DTOs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ElectionResponseDto>>> GetElections()
        {
            _logger.LogInformation("Handling GET /api/elections request.");
            var elections = await _service.GetAllElectionsAsync();
            return Ok(elections);
        }

        /// <summary>
        /// Retrieves a single election by ID, including its contests and candidates.
        /// </summary>
        /// <param name="id">The election ID.</param>
        /// <returns>The election response DTO if found; otherwise, 404 Not Found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ElectionResponseDto>> GetElection(int id)
        {
            _logger.LogInformation("Handling GET /api/elections/{Id} request.", id);
            var election = await _service.GetElectionByIdAsync(id);

            if (election == null)
            {
                return NotFound();
            }

            return Ok(election);
        }
    }
}
