namespace Coffeehouse.Api.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

/// <summary>
/// Controller for managing candidate profiles.
/// Uses literal routes and :int route constraints on id routes.
/// Protects write operations (POST, PUT, DELETE) with the CandidateProfileWrite policy.
/// GET operations remain public (AllowAnonymous).
/// </summary>
[ApiController]
[Route("api/candidateprofiles")]
[Route("api/candidates")]
public class CandidateProfilesController : ControllerBase
{
    private readonly ICandidateProfileService _service;
    private readonly ILogger<CandidateProfilesController> _logger;

    public CandidateProfilesController(ICandidateProfileService service, ILogger<CandidateProfilesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all candidate profiles, optionally filtered by district (ocdId).
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CandidateProfileResponseDto>>> GetAll([FromQuery] string? ocdId)
    {
        _logger.LogInformation("Handling GetAll candidate profiles request (ocdId: {OcdId})", ocdId);
        var profiles = await _service.GetAllProfilesAsync(ocdId);
        return Ok(profiles);
    }

    /// <summary>
    /// Retrieves a single candidate profile by integer ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<CandidateProfileResponseDto>> GetById(int id)
    {
        _logger.LogInformation("Handling GetById candidate profile request for ID {Id}", id);
        var profile = await _service.GetProfileAsync(id);
        if (profile == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Candidate Profile Not Found",
                Detail = $"Candidate profile with ID {id} was not found.",
                Status = StatusCodes.Status404NotFound
            });
        }

        return Ok(profile);
    }

    /// <summary>
    /// Creates a new candidate profile. Protected by CandidateProfileWrite policy.
    /// </summary>
    [HttpPost]
    [Authorize("CandidateProfileWrite")]
    public async Task<ActionResult<CandidateProfileResponseDto>> Create([FromBody] SaveCandidateProfileRequestDto request)
    {
        _logger.LogInformation("Handling Create candidate profile request for '{Name}'", request.Name);
        var outcome = await _service.CreateProfileAsync(request);

        switch (outcome.Status)
        {
            case ServiceOutcomeStatus.Success:
                _logger.LogInformation("Created candidate profile with ID {Id} for '{Name}'.", outcome.Data!.Id, outcome.Data.Name);
                return CreatedAtAction(nameof(GetById), new { id = outcome.Data!.Id }, outcome.Data);
            case ServiceOutcomeStatus.Duplicate:
                return Conflict(new ProblemDetails
                {
                    Title = "Duplicate Candidate Profile",
                    Detail = outcome.ErrorMessage,
                    Status = StatusCodes.Status409Conflict
                });
            case ServiceOutcomeStatus.InvalidInput:
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = outcome.ErrorMessage,
                    Status = StatusCodes.Status400BadRequest
                });
            default:
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = outcome.ErrorMessage,
                    Status = StatusCodes.Status400BadRequest
                });
        }
    }

    /// <summary>
    /// Updates an existing candidate profile by integer ID. Protected by CandidateProfileWrite policy.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize("CandidateProfileWrite")]
    public async Task<IActionResult> Update(int id, [FromBody] SaveCandidateProfileRequestDto request)
    {
        _logger.LogInformation("Handling Update candidate profile request for ID {Id}", id);
        var outcome = await _service.UpdateProfileAsync(id, request);

        switch (outcome.Status)
        {
            case ServiceOutcomeStatus.Success:
                _logger.LogInformation("Updated candidate profile with ID {Id}.", id);
                return NoContent();
            case ServiceOutcomeStatus.NotFound:
                return NotFound(new ProblemDetails
                {
                    Title = "Candidate Profile Not Found",
                    Detail = outcome.ErrorMessage,
                    Status = StatusCodes.Status404NotFound
                });
            case ServiceOutcomeStatus.Duplicate:
                return Conflict(new ProblemDetails
                {
                    Title = "Duplicate Candidate Profile",
                    Detail = outcome.ErrorMessage,
                    Status = StatusCodes.Status409Conflict
                });
            default:
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = outcome.ErrorMessage,
                    Status = StatusCodes.Status400BadRequest
                });
        }
    }

    /// <summary>
    /// Deletes a candidate profile by integer ID. Protected by CandidateProfileWrite policy.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize("CandidateProfileWrite")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Handling Delete candidate profile request for ID {Id}", id);
        var outcome = await _service.DeleteProfileAsync(id);

        switch (outcome.Status)
        {
            case ServiceOutcomeStatus.Success:
                _logger.LogInformation("Deleted candidate profile with ID {Id}.", id);
                return NoContent();
            case ServiceOutcomeStatus.NotFound:
                return NotFound(new ProblemDetails
                {
                    Title = "Candidate Profile Not Found",
                    Detail = outcome.ErrorMessage,
                    Status = StatusCodes.Status404NotFound
                });
            default:
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = outcome.ErrorMessage,
                    Status = StatusCodes.Status400BadRequest
                });
        }
    }

    /// <summary>
    /// Adds a video to a candidate profile. Protected by CandidateProfileWrite policy.
    /// </summary>
    [HttpPost("{id:int}/videos")]
    [Authorize("CandidateProfileWrite")]
    public async Task<ActionResult<VideoResponseDto>> AddVideo(int id, [FromBody] SaveVideoRequestDto request)
    {
        _logger.LogInformation("Handling AddVideo request for candidate profile ID {Id}", id);
        var outcome = await _service.AddVideoAsync(id, request);

        switch (outcome.Status)
        {
            case ServiceOutcomeStatus.Success:
                _logger.LogInformation("Added video to candidate profile ID {Id}.", id);
                return Ok(outcome.Data);
            case ServiceOutcomeStatus.NotFound:
                return NotFound(new ProblemDetails
                {
                    Title = "Candidate Profile Not Found",
                    Detail = outcome.ErrorMessage,
                    Status = StatusCodes.Status404NotFound
                });
            default:
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = outcome.ErrorMessage,
                    Status = StatusCodes.Status400BadRequest
                });
        }
    }
}
