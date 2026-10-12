using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Initializes a new instance of the ElectionsController.
        /// </summary>
        /// <param name="service">The election service.</param>
        public ElectionsController(IElectionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all elections including their contests.
        /// </summary>
        /// <returns>A list of election response DTOs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ElectionResponseDto>>> GetElections()
        {
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
            var election = await _service.GetElectionByIdAsync(id);

            if (election == null)
            {
                return NotFound();
            }

            return Ok(election);
        }
    }
}
