namespace Coffeehouse.Api.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.DTOs;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;

/// <summary>
/// Controller for managing candidate profiles.
/// Uses literal route [Route("api/candidateprofiles")] and :int route constraints on id routes.
/// Actions stay thin by calling the service once and mapping the outcome to status codes.
/// No business rules, no file access, and no try/catch blocks (handled globally).
/// </summary>
[ApiController]
[Route("api/candidateprofiles")]
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
    /// <param name="ocdId">Optional district identifier query parameter.</param>
    /// <returns>HTTP 200 with list of profiles.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CandidateProfileResponseDto>>> GetAll([FromQuery] string? ocdId)
    {
        _logger.LogInformation("Handling GetAll candidate profiles request (ocdId: {OcdId})", ocdId);
        var profiles = await _service.GetAllProfilesAsync(ocdId);
        return Ok(profiles);
    }

    /// <summary>
    /// Retrieves a single candidate profile by integer ID.
    /// </summary>
    /// <param name="id">The profile ID constraint to integer.</param>
    /// <returns>HTTP 200 with profile or HTTP 404 if not found.</returns>
    [HttpGet("{id:int}")]
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
    /// Creates a new candidate profile.
    /// Enforces DataAnnotations validation rules and duplicate name/ocdId business rule.
    /// </summary>
    /// <param name="request">The candidate profile request DTO.</param>
    /// <returns>HTTP 201 CreatedAtAction with Location header, HTTP 400 on invalid input, or HTTP 409 on duplicate.</returns>
    [HttpPost]
    public async Task<ActionResult<CandidateProfileResponseDto>> Create([FromBody] SaveCandidateProfileRequestDto request)
    {
        _logger.LogInformation("Handling Create candidate profile request for '{Name}'", request.Name);
        var outcome = await _service.CreateProfileAsync(request);

        return outcome.Status switch
        {
            ServiceOutcomeStatus.Success => CreatedAtAction(nameof(GetById), new { id = outcome.Data!.Id }, outcome.Data),
            ServiceOutcomeStatus.Duplicate => Conflict(new ProblemDetails
            {
                Title = "Duplicate Candidate Profile",
                Detail = outcome.ErrorMessage,
                Status = StatusCodes.Status409Conflict
            }),
            ServiceOutcomeStatus.InvalidInput => BadRequest(new ProblemDetails
            {
                Title = "Validation Error",
                Detail = outcome.ErrorMessage,
                Status = StatusCodes.Status400BadRequest
            }),
            _ => BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = outcome.ErrorMessage,
                Status = StatusCodes.Status400BadRequest
            })
        };
    }

    /// <summary>
    /// Updates an existing candidate profile by integer ID.
    /// </summary>
    /// <param name="id">The profile ID constraint to integer.</param>
    /// <param name="request">The updated profile request DTO.</param>
    /// <returns>HTTP 204 on success, HTTP 404 if not found, HTTP 400 on invalid input, or HTTP 409 on duplicate.</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SaveCandidateProfileRequestDto request)
    {
        _logger.LogInformation("Handling Update candidate profile request for ID {Id}", id);
        var outcome = await _service.UpdateProfileAsync(id, request);

        return outcome.Status switch
        {
            ServiceOutcomeStatus.Success => NoContent(),
            ServiceOutcomeStatus.NotFound => NotFound(new ProblemDetails
            {
                Title = "Candidate Profile Not Found",
                Detail = outcome.ErrorMessage,
                Status = StatusCodes.Status404NotFound
            }),
            ServiceOutcomeStatus.Duplicate => Conflict(new ProblemDetails
            {
                Title = "Duplicate Candidate Profile",
                Detail = outcome.ErrorMessage,
                Status = StatusCodes.Status409Conflict
            }),
            _ => BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = outcome.ErrorMessage,
                Status = StatusCodes.Status400BadRequest
            })
        };
    }

    /// <summary>
    /// Deletes a candidate profile by integer ID.
    /// </summary>
    /// <param name="id">The profile ID constraint to integer.</param>
    /// <returns>HTTP 204 on success or HTTP 404 if not found.</returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Handling Delete candidate profile request for ID {Id}", id);
        var outcome = await _service.DeleteProfileAsync(id);

        return outcome.Status switch
        {
            ServiceOutcomeStatus.Success => NoContent(),
            ServiceOutcomeStatus.NotFound => NotFound(new ProblemDetails
            {
                Title = "Candidate Profile Not Found",
                Detail = outcome.ErrorMessage,
                Status = StatusCodes.Status404NotFound
            }),
            _ => BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = outcome.ErrorMessage,
                Status = StatusCodes.Status400BadRequest
            })
        };
    }

    /// <summary>
    /// Adds a video to a candidate profile.
    /// </summary>
    [HttpPost("{id:int}/videos")]
    public async Task<ActionResult<VideoResponseDto>> AddVideo(int id, [FromBody] SaveVideoRequestDto request)
    {
        _logger.LogInformation("Handling AddVideo request for candidate profile ID {Id}", id);
        var outcome = await _service.AddVideoAsync(id, request);

        return outcome.Status switch
        {
            ServiceOutcomeStatus.Success => Ok(outcome.Data),
            ServiceOutcomeStatus.NotFound => NotFound(new ProblemDetails
            {
                Title = "Candidate Profile Not Found",
                Detail = outcome.ErrorMessage,
                Status = StatusCodes.Status404NotFound
            }),
            _ => BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Detail = outcome.ErrorMessage,
                Status = StatusCodes.Status400BadRequest
            })
        };
    }
}
