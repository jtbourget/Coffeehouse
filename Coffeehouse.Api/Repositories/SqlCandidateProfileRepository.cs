namespace Coffeehouse.Api.Repositories;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Coffeehouse.Api.Data;
using Coffeehouse.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// SQL-based implementation of the candidate profile repository using Entity Framework Core.
/// Injected as a Singleton: uses IServiceScopeFactory to safely obtain scoped DbContext instances.
/// </summary>
public class SqlCandidateProfileRepository : ICandidateProfileRepository
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SqlCandidateProfileRepository(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<IEnumerable<CandidateProfile>> GetAllProfilesAsync(string? ocdId = null)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var query = context.CandidateProfiles
            .Include(p => p.Videos)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(ocdId))
        {
            var trimmedOcdId = ocdId.Trim().ToLower();
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
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<CandidateProfile?> GetByNameAndOcdIdAsync(string name, string ocdId)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.CandidateProfiles
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower() && p.OcdId.ToLower() == ocdId.ToLower());
    }

    public async Task<CandidateProfile> CreateProfileAsync(CandidateProfile profile)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.CandidateProfiles.Add(profile);
        await context.SaveChangesAsync();
        await SyncToFileAsync(context);
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
        await SyncToFileAsync(context);
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
        await SyncToFileAsync(context);
        return true;
    }

    private static async Task SyncToFileAsync(AppDbContext context)
    {
        try
        {
            var dataDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            var filePath = Path.Combine(dataDir, "CandidateProfiles.json");
            var profiles = await context.CandidateProfiles
                .AsNoTracking()
                .ToListAsync();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(profiles, options);
            await File.WriteAllTextAsync(filePath, json);
        }
        catch
        {
            // Logging or fallback if file access encounters permission restrictions
        }
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
