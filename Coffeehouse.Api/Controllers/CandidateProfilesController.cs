using Coffeehouse.Api.Models.DTOs;
using Coffeehouse.Api.Repositories;
using Coffeehouse.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Controllers;

/// <summary>
/// Controller for managing candidate profiles and associated media.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CandidateProfilesController : ControllerBase
{
    private readonly ICandidateProfileRepository _repository;
    private readonly ICandidateProfileMapper _mapper;

    public CandidateProfilesController(ICandidateProfileRepository repository, ICandidateProfileMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CandidateProfileResponseDto>> GetProfile(int id)
    {
        var profile = await _repository.GetProfileAsync(id);

        if (profile == null) return NotFound();

        return _mapper.ToDto(profile);
    }

    [HttpPost]
    public async Task<ActionResult<CandidateProfileResponseDto>> CreateProfile(SaveCandidateProfileRequestDto request)
    {
        var profile = _mapper.ToEntity(request);
        var createdProfile = await _repository.CreateProfileAsync(profile);

        return CreatedAtAction(nameof(GetProfile), new { id = createdProfile.Id }, _mapper.ToDto(createdProfile));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(int id, SaveCandidateProfileRequestDto request)
    {
        if (id != request.Id) return BadRequest();

        var profile = _mapper.ToEntity(request);

        try
        {
            await _repository.UpdateProfileAsync(profile);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _repository.ProfileExistsAsync(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpPost("{id}/videos")]
    public async Task<ActionResult<VideoResponseDto>> AddVideo(int id, SaveVideoRequestDto request)
    {
        if (!await _repository.ProfileExistsAsync(id)) return NotFound("Candidate profile not found.");

        var video = _mapper.ToEntity(request);
        var addedVideo = await _repository.AddVideoAsync(id, video);

        return Ok(_mapper.ToDto(addedVideo));
    }
}
