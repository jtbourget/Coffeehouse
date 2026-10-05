using System;
using System.Threading.Tasks;
using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Coffeehouse.Api.Repositories;

public class SqlCandidateProfileRepository : ICandidateProfileRepository
{
    private readonly AppDbContext _context;

    public SqlCandidateProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CandidateProfile?> GetProfileAsync(int id)
    {
        return await _context.CandidateProfiles
            .Include(p => p.Videos)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<CandidateProfile> CreateProfileAsync(CandidateProfile profile)
    {
        _context.CandidateProfiles.Add(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task UpdateProfileAsync(CandidateProfile profile)
    {
        _context.Entry(profile).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<Video> AddVideoAsync(int candidateProfileId, Video video)
    {
        video.CandidateProfileId = candidateProfileId;
        video.UploadedAt = DateTime.UtcNow;
        _context.Videos.Add(video);
        await _context.SaveChangesAsync();
        return video;
    }

    public async Task<bool> ProfileExistsAsync(int id)
    {
        return await _context.CandidateProfiles.AnyAsync(e => e.Id == id);
    }
}
