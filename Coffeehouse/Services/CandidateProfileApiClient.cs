using System.Net;
using System.Text;
using System.Text.Json;
using Coffeehouse.Models;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Services;

public class CandidateProfileApiClient : ICandidateProfileApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CandidateProfileApiClient> _logger;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public CandidateProfileApiClient(HttpClient httpClient, ILogger<CandidateProfileApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ApiResult<List<CandidateProfileDisplay>>> GetProfilesAsync(string? ocdId = null, string? search = null)
    {
        _logger.LogInformation("Calling GET api/candidateprofiles (ocdId: {OcdId})", ocdId);

        try
        {
            var url = "api/candidateprofiles";
            if (!string.IsNullOrWhiteSpace(ocdId))
            {
                url += $"?ocdId={Uri.EscapeDataString(ocdId.Trim())}";
            }

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var profiles = JsonSerializer.Deserialize<List<CandidateProfileDisplay>>(content, _jsonOptions) ?? new();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var term = search.Trim();
                    profiles = profiles.Where(p =>
                        p.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        p.OcdId.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        (p.Bio != null && p.Bio.Contains(term, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                return ApiResult<List<CandidateProfileDisplay>>.Success(profiles, response.StatusCode);
            }

            var problem = await TryReadProblemDetailsAsync(response);
            return ApiResult<List<CandidateProfileDisplay>>.Failure(response.StatusCode, problem);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network failure reaching api/candidateprofiles.");
            return ApiResult<List<CandidateProfileDisplay>>.Failure(HttpStatusCode.ServiceUnavailable, networkError: "Can't reach the API. Is it running?");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Timeout reaching api/candidateprofiles.");
            return ApiResult<List<CandidateProfileDisplay>>.Failure(HttpStatusCode.RequestTimeout, networkError: "Can't reach the API. Is it running?");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting candidate profiles.");
            return ApiResult<List<CandidateProfileDisplay>>.Failure(HttpStatusCode.InternalServerError, networkError: ex.Message);
        }
    }

    public async Task<ApiResult<CandidateProfileDisplay>> GetProfileAsync(int id)
    {
        _logger.LogInformation("Calling GET api/candidateprofiles/{Id}", id);

        try
        {
            var response = await _httpClient.GetAsync($"api/candidateprofiles/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var profile = JsonSerializer.Deserialize<CandidateProfileDisplay>(content, _jsonOptions);
                return profile != null
                    ? ApiResult<CandidateProfileDisplay>.Success(profile, response.StatusCode)
                    : ApiResult<CandidateProfileDisplay>.Failure(HttpStatusCode.NotFound);
            }

            var problem = await TryReadProblemDetailsAsync(response);
            return ApiResult<CandidateProfileDisplay>.Failure(response.StatusCode, problem);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network failure reaching api/candidateprofiles/{Id}.", id);
            return ApiResult<CandidateProfileDisplay>.Failure(HttpStatusCode.ServiceUnavailable, networkError: "Can't reach the API. Is it running?");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Timeout reaching api/candidateprofiles/{Id}.", id);
            return ApiResult<CandidateProfileDisplay>.Failure(HttpStatusCode.RequestTimeout, networkError: "Can't reach the API. Is it running?");
        }
    }

    public async Task<ApiResult<CandidateProfileDisplay>> CreateProfileAsync(SaveCandidateProfileDto dto)
    {
        _logger.LogInformation("Calling POST api/candidateprofiles for '{Name}'", dto.Name);

        try
        {
            var json = JsonSerializer.Serialize(dto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/candidateprofiles", content);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var created = JsonSerializer.Deserialize<CandidateProfileDisplay>(body, _jsonOptions);
                return created != null
                    ? ApiResult<CandidateProfileDisplay>.Success(created, response.StatusCode)
                    : ApiResult<CandidateProfileDisplay>.Success(new CandidateProfileDisplay { Name = dto.Name, OcdId = dto.OcdId }, response.StatusCode);
            }

            var problem = await TryReadProblemDetailsAsync(response);
            return ApiResult<CandidateProfileDisplay>.Failure(response.StatusCode, problem);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network failure creating candidate profile.");
            return ApiResult<CandidateProfileDisplay>.Failure(HttpStatusCode.ServiceUnavailable, networkError: "Can't reach the API. Is it running?");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Timeout creating candidate profile.");
            return ApiResult<CandidateProfileDisplay>.Failure(HttpStatusCode.RequestTimeout, networkError: "Can't reach the API. Is it running?");
        }
    }

    public async Task<ApiResult> UpdateProfileAsync(int id, SaveCandidateProfileDto dto)
    {
        _logger.LogInformation("Calling PUT api/candidateprofiles/{Id}", id);

        try
        {
            var json = JsonSerializer.Serialize(dto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/candidateprofiles/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                return ApiResult.NoContent();
            }

            var problem = await TryReadProblemDetailsAsync(response);
            return ApiResult.Failure(response.StatusCode, problem);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network failure updating candidate profile {Id}.", id);
            return ApiResult.Failure(HttpStatusCode.ServiceUnavailable, networkError: "Can't reach the API. Is it running?");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Timeout updating candidate profile {Id}.", id);
            return ApiResult.Failure(HttpStatusCode.RequestTimeout, networkError: "Can't reach the API. Is it running?");
        }
    }

    public async Task<ApiResult> DeleteProfileAsync(int id)
    {
        _logger.LogInformation("Calling DELETE api/candidateprofiles/{Id}", id);

        try
        {
            var response = await _httpClient.DeleteAsync($"api/candidateprofiles/{id}");
            if (response.IsSuccessStatusCode)
            {
                return ApiResult.NoContent();
            }

            var problem = await TryReadProblemDetailsAsync(response);
            return ApiResult.Failure(response.StatusCode, problem);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network failure deleting candidate profile {Id}.", id);
            return ApiResult.Failure(HttpStatusCode.ServiceUnavailable, networkError: "Can't reach the API. Is it running?");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Timeout deleting candidate profile {Id}.", id);
            return ApiResult.Failure(HttpStatusCode.RequestTimeout, networkError: "Can't reach the API. Is it running?");
        }
    }

    private async Task<ProblemDetailsDto?> TryReadProblemDetailsAsync(HttpResponseMessage response)
    {
        try
        {
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content)) return null;

            return JsonSerializer.Deserialize<ProblemDetailsDto>(content, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }
}
