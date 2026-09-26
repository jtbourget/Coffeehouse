using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidateProfilesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CandidateProfilesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CandidateProfile>> GetProfile(int id)
    {
        var profile = await _context.CandidateProfiles
            .Include(p => p.Videos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (profile == null) return NotFound();

        return profile;
    }

    [HttpPost]
    public async Task<ActionResult<CandidateProfile>> CreateProfile(CandidateProfile profile)
    {
        _context.CandidateProfiles.Add(profile);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProfile), new { id = profile.Id }, profile);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(int id, CandidateProfile profile)
    {
        if (id != profile.Id) return BadRequest();

        _context.Entry(profile).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProfileExists(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpPost("{id}/videos")]
    public async Task<ActionResult<Video>> AddVideo(int id, Video video)
    {
        var profile = await _context.CandidateProfiles.FindAsync(id);
        if (profile == null) return NotFound("Candidate profile not found.");

        video.CandidateProfileId = id;
        video.UploadedAt = DateTime.UtcNow;

        _context.Videos.Add(video);
        await _context.SaveChangesAsync();

        return Ok(video);
    }

    private bool ProfileExists(int id)
    {
        return _context.CandidateProfiles.Any(e => e.Id == id);
    }
}
