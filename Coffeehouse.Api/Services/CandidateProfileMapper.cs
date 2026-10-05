using System.Linq;
using Coffeehouse.Api.Models;
using Coffeehouse.Api.Models.DTOs;

namespace Coffeehouse.Api.Services;

public class CandidateProfileMapper : ICandidateProfileMapper
{
    public CandidateProfile ToEntity(SaveCandidateProfileRequestDto dto)
    {
        return new CandidateProfile
        {
            Id = dto.Id,
            OcdId = dto.OcdId,
            Name = dto.Name,
            Bio = dto.Bio,
            WebsiteUrl = dto.WebsiteUrl
        };
    }

    public CandidateProfileResponseDto ToDto(CandidateProfile entity)
    {
        return new CandidateProfileResponseDto
        {
            Id = entity.Id,
            OcdId = entity.OcdId,
            Name = entity.Name,
            Bio = entity.Bio,
            WebsiteUrl = entity.WebsiteUrl,
            Videos = entity.Videos?.Select(ToDto).ToList() ?? new()
        };
    }

    public Video ToEntity(SaveVideoRequestDto dto)
    {
        return new Video
        {
            Title = dto.Title,
            VideoUrl = dto.VideoUrl
        };
    }

    public VideoResponseDto ToDto(Video entity)
    {
        return new VideoResponseDto
        {
            Id = entity.Id,
            Title = entity.Title,
            VideoUrl = entity.VideoUrl,
            UploadedAt = entity.UploadedAt,
            CandidateProfileId = entity.CandidateProfileId
        };
    }
}
