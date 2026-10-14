using System.Text.Json;
using System.Text;
using Coffeehouse.Models;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Coffeehouse.Services
{
    /// <summary>
    /// Service to interact with the backend API.
    /// Implemented as a singleton.
    /// </summary>
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<ApiService> _logger;

        public ApiService(IHttpClientFactory factory, ILogger<ApiService> logger)
        {
            _httpClient = factory.CreateClient("CoffeehouseApi");
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _logger = logger;
        }

        /// <summary>
        /// Gets the user's saved address.
        /// </summary>
        public async Task<UserAddress?> GetAddressAsync()
        {
            _logger.LogDebug("Calling GET /api/address");
            try
            {
                var response = await _httpClient.GetAsync("/api/address");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogDebug("GET /api/address returned 404 Not Found.");
                    return null;
                }
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    return JsonSerializer.Deserialize<UserAddress>(content, _jsonOptions);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user address from API.");
                throw;
            }
        }

        /// <summary>
        /// Saves or updates the user's address.
        /// </summary>
        public async Task<bool> SaveAddressAsync(UserAddress address)
        {
            _logger.LogInformation("Calling POST /api/address for {City}, {State}", address.City, address.State);
            try
            {
                var json = JsonSerializer.Serialize(address, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/address", content);
                response.EnsureSuccessStatusCode();
                _logger.LogInformation("Successfully saved address to API.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save address to API.");
                throw;
            }
        }

        /// <summary>
        /// Gets a list of all elections.
        /// </summary>
        public async Task<List<Election>> GetElectionsAsync()
        {
            _logger.LogDebug("Calling GET /api/civic/elections");
            try
            {
                var response = await _httpClient.GetAsync("/api/civic/elections");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var elections = JsonSerializer.Deserialize<List<Election>>(content, _jsonOptions) ?? new();
                _logger.LogDebug("GET /api/civic/elections returned {Count} elections.", elections.Count);
                return elections;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve elections from API.");
                throw;
            }
        }

        /// <summary>
        /// Gets a specific election by ID.
        /// </summary>
        public async Task<Election?> GetElectionAsync(int id)
        {
            _logger.LogDebug("Calling GET /api/civic/elections/{Id}", id);
            try
            {
                var response = await _httpClient.GetAsync($"/api/civic/elections/{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogDebug("GET /api/civic/elections/{Id} returned 404 Not Found.", id);
                    return null;
                }
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Election>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve election {Id} from API.", id);
                throw;
            }
        }

        /// <summary>
        /// Gets contests for a given election.
        /// </summary>
        public async Task<List<Contest>> GetContestsAsync(int electionId)
        {
            _logger.LogDebug("Calling GET /api/civic/elections/{ElectionId}/contests", electionId);
            try
            {
                var response = await _httpClient.GetAsync($"/api/civic/elections/{electionId}/contests");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var contests = JsonSerializer.Deserialize<List<Contest>>(content, _jsonOptions) ?? new();
                _logger.LogDebug("Retrieved {Count} contests for election {ElectionId}.", contests.Count, electionId);
                return contests;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve contests for election {ElectionId}.", electionId);
                throw;
            }
        }

        /// <summary>
        /// Gets candidates for a given contest.
        /// </summary>
        public async Task<List<Candidate>> GetCandidatesAsync(int contestId)
        {
            _logger.LogDebug("Calling GET /api/civic/contests/{ContestId}/candidates", contestId);
            try
            {
                var response = await _httpClient.GetAsync($"/api/civic/contests/{contestId}/candidates");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var candidates = JsonSerializer.Deserialize<List<Candidate>>(content, _jsonOptions) ?? new();
                _logger.LogDebug("Retrieved {Count} candidates for contest {ContestId}.", candidates.Count, contestId);
                return candidates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve candidates for contest {ContestId}.", contestId);
                throw;
            }
        }

        /// <summary>
        /// Gets a specific candidate by ID.
        /// </summary>
        public async Task<Candidate?> GetCandidateAsync(int id)
        {
            _logger.LogDebug("Calling GET /api/civic/candidates/{Id}", id);
            try
            {
                var response = await _httpClient.GetAsync($"/api/civic/candidates/{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogDebug("GET /api/civic/candidates/{Id} returned 404 Not Found.", id);
                    return null;
                }
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Candidate>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve candidate {Id} from API.", id);
                throw;
            }
        }

        /// <summary>
        /// Gets all favorited candidates for an election.
        /// </summary>
        public async Task<List<FavoriteCandidate>> GetFavoritesAsync(int electionId)
        {
            _logger.LogDebug("Calling GET /api/elections/{ElectionId}/favorites", electionId);
            try
            {
                var response = await _httpClient.GetAsync($"/api/elections/{electionId}/favorites");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<FavoriteCandidate>>(content, _jsonOptions) ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve favorites for election {ElectionId}.", electionId);
                throw;
            }
        }

        /// <summary>
        /// Sets a candidate as favorite for a contest.
        /// </summary>
        public async Task<bool> SetFavoriteAsync(int contestId, int candidateId)
        {
            _logger.LogInformation("Calling PUT /api/favorites/{ContestId}/{CandidateId}", contestId, candidateId);
            try
            {
                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/favorites/{contestId}/{candidateId}", content);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set favorite candidate {CandidateId} for contest {ContestId}.", candidateId, contestId);
                throw;
            }
        }

        /// <summary>
        /// Removes the favorite candidate for a contest.
        /// </summary>
        public async Task<bool> RemoveFavoriteAsync(int contestId)
        {
            _logger.LogInformation("Calling DELETE /api/favorites/{ContestId}", contestId);
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/favorites/{contestId}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove favorite for contest {ContestId}.", contestId);
                throw;
            }
        }

        public async Task<List<AddressSuggestion>> GetAddressSuggestionsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return new List<AddressSuggestion>();

            _logger.LogDebug("Calling GET /api/places/autocomplete for query: {Query}", query);
            try
            {
                var response = await _httpClient.GetAsync($"/api/places/autocomplete?query={Uri.EscapeDataString(query)}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var suggestions = JsonSerializer.Deserialize<List<AddressSuggestion>>(content, _jsonOptions) ?? new();
                _logger.LogDebug("Retrieved {Count} address suggestions.", suggestions.Count);
                return suggestions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get address suggestions for query '{Query}'.", query);
                throw;
            }
        }

        public async Task<UserAddress?> GetPlaceDetailsAsync(string placeId)
        {
            _logger.LogDebug("Calling GET /api/places/details/{PlaceId}", placeId);
            try
            {
                var response = await _httpClient.GetAsync($"/api/places/details/{placeId}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogDebug("Place details for {PlaceId} returned 404 Not Found.", placeId);
                    return null;
                }
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserAddress>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get place details for {PlaceId}.", placeId);
                throw;
            }
        }
    }
}
