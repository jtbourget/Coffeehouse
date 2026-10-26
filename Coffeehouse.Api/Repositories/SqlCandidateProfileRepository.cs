namespace Coffeehouse.Api.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

/// <summary>
/// SQL-based implementation of the candidate profile repository using Entity Framework Core.
/// </summary>
public class SqlCandidateProfileRepository : ICandidateProfileRepository
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SqlCandidateProfileRepository> _logger;

    public SqlCandidateProfileRepository(IServiceScopeFactory scopeFactory, ILogger<SqlCandidateProfileRepository> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<CandidateProfile>> GetAllProfilesAsync(string? ocdId = null)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var query = context.CandidateProfiles
            .Include(p => p.Videos)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(ocdId))
        {
            var trimmedOcdId = ocdId.Trim().ToLowerInvariant();
            query = query.Where(p => p.OcdId.ToLower() == trimmedOcdId);
        }

        return await query.ToListAsync();
    }

    public async Task<CandidateProfile?> GetProfileAsync(int id)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.CandidateProfiles
            .Include(p => p.Videos)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<CandidateProfile?> GetByNameAndOcdIdAsync(string name, string ocdId)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var nameLower = name.ToLowerInvariant();
        var ocdIdLower = ocdId.ToLowerInvariant();
        return await context.CandidateProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name.ToLower() == nameLower && p.OcdId.ToLower() == ocdIdLower);
    }

    public async Task<CandidateProfile> CreateProfileAsync(CandidateProfile profile)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.CandidateProfiles.Add(profile);
        await context.SaveChangesAsync();
        _logger.LogInformation("SQL Store: Created candidate profile {Id} ('{Name}')", profile.Id, profile.Name);
        return profile;
    }

    public async Task<bool> UpdateProfileAsync(int id, CandidateProfile updated)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var existing = await context.CandidateProfiles.FindAsync(id);
        if (existing == null)
        {
            return false;
        }

        existing.Name = updated.Name;
        existing.OcdId = updated.OcdId;
        existing.Bio = updated.Bio;
        existing.WebsiteUrl = updated.WebsiteUrl;
        await context.SaveChangesAsync();
        _logger.LogInformation("SQL Store: Updated candidate profile {Id}", id);
        return true;
    }

    public async Task<bool> DeleteProfileAsync(int id)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var existing = await context.CandidateProfiles.FindAsync(id);
        if (existing == null)
        {
            return false;
        }

        context.CandidateProfiles.Remove(existing);
        await context.SaveChangesAsync();
        _logger.LogInformation("SQL Store: Deleted candidate profile {Id}", id);
        return true;
    }

    public async Task<Video?> AddVideoAsync(int candidateProfileId, Video video)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var profileExists = await context.CandidateProfiles.AnyAsync(p => p.Id == candidateProfileId);
        if (!profileExists)
        {
            return null;
        }

        video.CandidateProfileId = candidateProfileId;
        video.UploadedAt = DateTime.UtcNow;
        context.Videos.Add(video);
        await context.SaveChangesAsync();
        return video;
    }

    public async Task<bool> ProfileExistsAsync(int id)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.CandidateProfiles.AnyAsync(e => e.Id == id);
    }
}
