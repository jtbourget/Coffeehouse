using System.Text.Json;
using System.Text.Json.Serialization;
using Coffeehouse.Api.Models;

namespace Coffeehouse.Api.Data;

public sealed class JsonStoreState : IDisposable
{
    private readonly string _filePath;
    private readonly string _lockPath;
    private readonly FileStream _lockStream;
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
    };

    private List<CandidateProfile> _profiles = new();
    private int _nextId = 1;

    public JsonStoreState(string filePath)
    {
        _filePath = filePath;
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

        _lockPath = _filePath + ".lock";
        _lockStream = new FileStream(_lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
    }

    public void InitializeFromSeed(string seedFilePath)
    {
        lock (_lockStream)
        {
            if (File.Exists(_filePath))
            {
                Load();
                return;
            }

            if (File.Exists(seedFilePath))
            {
                ReplaceWithSeed(seedFilePath);
            }
            else
            {
                _profiles = new List<CandidateProfile>();
                _nextId = 1;
                Save();
            }
        }
    }

    public IReadOnlyList<CandidateProfile> Snapshot()
    {
        lock (_lockStream)
        {
            Load();
            return _profiles.Select(Clone).ToList();
        }
    }

    public CandidateProfile? Find(int id)
    {
        lock (_lockStream)
        {
            Load();
            var profile = _profiles.FirstOrDefault(p => p.Id == id);
            return profile is null ? null : Clone(profile);
        }
    }

    public CandidateProfile? FindByNameAndOcdId(string name, string ocdId)
    {
        lock (_lockStream)
        {
            Load();
            var profile = _profiles.FirstOrDefault(p =>
                string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(p.OcdId, ocdId, StringComparison.OrdinalIgnoreCase));
            return profile is null ? null : Clone(profile);
        }
    }

    public CandidateProfile Add(CandidateProfile profile)
    {
        lock (_lockStream)
        {
            Load();
            profile.Id = _nextId++;
            if (profile.CreatedAt == default)
            {
                profile.CreatedAt = DateTime.UtcNow;
            }
            _profiles.Add(Clone(profile));
            Save();
            return Clone(profile);
        }
    }

    public bool Update(int id, CandidateProfile replacement)
    {
        lock (_lockStream)
        {
            Load();
            var index = _profiles.FindIndex(p => p.Id == id);
            if (index < 0) return false;

            var existing = _profiles[index];
            existing.Name = replacement.Name;
            existing.OcdId = replacement.OcdId;
            existing.Bio = replacement.Bio;
            existing.WebsiteUrl = replacement.WebsiteUrl;
            Save();
            return true;
        }
    }

    public bool Remove(int id)
    {
        lock (_lockStream)
        {
            Load();
            var index = _profiles.FindIndex(p => p.Id == id);
            if (index < 0) return false;

            _profiles.RemoveAt(index);
            Save();
            return true;
        }
    }

    public Video? AddVideo(int candidateProfileId, Video video)
    {
        lock (_lockStream)
        {
            Load();
            var profile = _profiles.FirstOrDefault(p => p.Id == candidateProfileId);
            if (profile == null) return null;

            var maxVideoId = _profiles.SelectMany(p => p.Videos).Select(v => v.Id).DefaultIfEmpty(0).Max();
            video.Id = maxVideoId + 1;
            video.CandidateProfileId = candidateProfileId;
            video.UploadedAt = DateTime.UtcNow;
            profile.Videos.Add(video);
            Save();
            return video;
        }
    }

    public bool Exists(int id)
    {
        lock (_lockStream)
        {
            Load();
            return _profiles.Any(p => p.Id == id);
        }
    }

    public void ReplaceWithSeed(string seedFilePath)
    {
        lock (_lockStream)
        {
            var json = File.ReadAllText(seedFilePath);
            var loaded = DeserializeProfiles(json);
            _profiles = loaded;
            _nextId = _profiles.Count > 0 ? _profiles.Max(p => p.Id) + 1 : 1;
            Save();
        }
    }

    public void Dispose()
    {
        _lockStream.Dispose();
        try
        {
            File.Delete(_lockPath);
        }
        catch
        {
            // Lock file cleanup is best-effort
        }
    }

    private void Load()
    {
        if (!File.Exists(_filePath))
        {
            _profiles = new List<CandidateProfile>();
            _nextId = 1;
            return;
        }

        var json = File.ReadAllText(_filePath);
        _profiles = DeserializeProfiles(json);
        _nextId = _profiles.Count > 0 ? _profiles.Max(p => p.Id) + 1 : 1;
    }

    private List<CandidateProfile> DeserializeProfiles(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return new List<CandidateProfile>();

        try
        {
            var list = JsonSerializer.Deserialize<List<CandidateProfile>>(json, _options);
            if (list != null) return list;
        }
        catch
        {
            var envelope = JsonSerializer.Deserialize<StoreEnvelope>(json, _options);
            if (envelope?.Profiles != null) return envelope.Profiles;
        }

        return new List<CandidateProfile>();
    }

    private void Save()
    {
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_profiles, _options));
    }

    private static CandidateProfile Clone(CandidateProfile source) =>
        JsonSerializer.Deserialize<CandidateProfile>(JsonSerializer.Serialize(source))!;

    private class StoreEnvelope
    {
        [JsonPropertyName("schemaVersion")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("nextId")]
        public int NextId { get; set; } = 1;

        [JsonPropertyName("profiles")]
        public List<CandidateProfile> Profiles { get; set; } = new();
    }
}
