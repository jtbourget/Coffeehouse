using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Api.Controllers;

/// <summary>
/// Controller for managing civic data, elections, contests, and candidates.
/// </summary>
[ApiController]
[Route("api/civic")]
public class CivicController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly Services.GeocodioService _civicService;
    private readonly ILogger<CivicController> _logger;

    public CivicController(AppDbContext context, Services.GeocodioService civicService, ILogger<CivicController> logger)
    {
        _context = context;
        _civicService = civicService;
        _logger = logger;
    }

    [HttpGet("elections")]
    public async Task<ActionResult<IEnumerable<Election>>> GetElections()
    {
        _logger.LogInformation("Handling GET /api/civic/elections request.");
        var elections = await _context.Elections
            .Include(e => e.Contests)
                .ThenInclude(c => c.Candidates)
                    .ThenInclude(c => c.Videos)
            .ToListAsync();

        // 1. Get user address
        var address = await _context.UserAddresses.OrderByDescending(a => a.Id).FirstOrDefaultAsync();
        if (address != null)
        {
            var addressString = $"{address.Street}, {address.City}, {address.State} {address.ZipCode}";
            
            // 2. Fetch matching OCD-IDs from Google Civic API
            var userOcdIds = await _civicService.GetOcdIdsForAddressAsync(addressString);
            
            // 3. Filter contests for each election
            foreach (var election in elections)
            {
                // Only keep contests that match the user's OCD IDs (or have no OCD ID set)
                election.Contests = election.Contests
                    .Where(c => string.IsNullOrEmpty(c.OcdId) || userOcdIds.Contains(c.OcdId))
                    .ToList();
            }
        }

        // Merge in local CandidateProfiles data
        var profilesList = await _context.CandidateProfiles.Include(p => p.Videos).ToListAsync();
        foreach(var e in elections)
        {
            foreach(var c in e.Contests)
            {
                foreach(var cand in c.Candidates)
                {
                    var profile = profilesList.FirstOrDefault(p => p.Name.Equals(cand.Name, StringComparison.OrdinalIgnoreCase));
                    if(profile != null)
                    {
                        if(string.IsNullOrEmpty(cand.Bio) && profile.Bio != null) cand.Bio = profile.Bio;
                        if(string.IsNullOrEmpty(cand.CampaignWebsite) && profile.WebsiteUrl != null) cand.CampaignWebsite = profile.WebsiteUrl;
                        if(!cand.Videos.Any() && profile.Videos != null) cand.Videos = profile.Videos;
                    }
                }
            }
        }

        return Ok(elections);
    }

    [HttpGet("elections/{id}")]
    public async Task<ActionResult<Election>> GetElection(int id)
    {
        _logger.LogInformation("Handling GET /api/civic/elections/{Id} request.", id);
        var electionsResult = await GetElections();
        var okResult = electionsResult.Result as OkObjectResult;
        if (okResult?.Value is IEnumerable<Election> elections)
        {
            var election = elections.FirstOrDefault(e => e.Id == id);
            if (election != null) return Ok(election);
        }
        return NotFound();
    }

    [HttpGet("elections/{id}/contests")]
    public async Task<ActionResult<IEnumerable<Contest>>> GetContests(int id)
    {
        _logger.LogInformation("Handling GET /api/civic/elections/{Id}/contests request.", id);
        var electionResult = await GetElection(id);
        if (electionResult.Result is OkObjectResult okResult && okResult.Value is Election election)
        {
            return Ok(election.Contests);
        }
        return NotFound();
    }

    [HttpGet("contests/{contestId}/candidates")]
    public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidates(int contestId)
    {
        _logger.LogInformation("Handling GET /api/civic/contests/{ContestId}/candidates request.", contestId);
        var electionsResult = await GetElections();
        var okResult = electionsResult.Result as OkObjectResult;
        if (okResult?.Value is IEnumerable<Election> elections)
        {
            foreach (var election in elections)
            {
                var contest = election.Contests.FirstOrDefault(c => c.Id == contestId);
                if (contest != null) return Ok(contest.Candidates);
            }
        }
        return NotFound();
    }

    [HttpGet("candidates/{id}")]
    public async Task<ActionResult<Candidate>> GetCandidate(int id)
    {
        _logger.LogInformation("Handling GET /api/civic/candidates/{Id} request.", id);
        var electionsResult = await GetElections();
        var okResult = electionsResult.Result as OkObjectResult;
        if (okResult?.Value is IEnumerable<Election> elections)
        {
            foreach (var election in elections)
            {
                foreach (var contest in election.Contests)
                {
                    var candidate = contest.Candidates.FirstOrDefault(c => c.Id == id);
                    if (candidate != null) return Ok(candidate);
                }
            }
        }
        return NotFound();
    }
}
