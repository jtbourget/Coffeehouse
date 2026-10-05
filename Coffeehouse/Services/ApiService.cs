using System.Text.Json;
using System.Text;
using Coffeehouse.Models;
using System.Diagnostics;
using System.Net.Http;

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

        public ApiService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("CoffeehouseApi");
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        /// <summary>
        /// Gets the user's saved address.
        /// </summary>
        public async Task<UserAddress?> GetAddressAsync()
        {
            var response = await _httpClient.GetAsync("/api/address");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(content))
            {
                return JsonSerializer.Deserialize<UserAddress>(content, _jsonOptions);
            }
            return null;
        }

        /// <summary>
        /// Saves or updates the user's address.
        /// </summary>
        public async Task<bool> SaveAddressAsync(UserAddress address)
        {
            var json = JsonSerializer.Serialize(address, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/address", content);
            response.EnsureSuccessStatusCode();
            return true;
        }

        /// <summary>
        /// Gets a list of all elections.
        /// </summary>
        public async Task<List<Election>> GetElectionsAsync()
        {
            var response = await _httpClient.GetAsync("/api/civic/elections");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Election>>(content, _jsonOptions) ?? new();
        }

        /// <summary>
        /// Gets a specific election by ID.
        /// </summary>
        public async Task<Election?> GetElectionAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/civic/elections/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Election>(content, _jsonOptions);
        }

        /// <summary>
        /// Gets contests for a given election.
        /// </summary>
        public async Task<List<Contest>> GetContestsAsync(int electionId)
        {
            var response = await _httpClient.GetAsync($"/api/civic/elections/{electionId}/contests");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Contest>>(content, _jsonOptions) ?? new();
        }

        /// <summary>
        /// Gets candidates for a given contest.
        /// </summary>
        public async Task<List<Candidate>> GetCandidatesAsync(int contestId)
        {
            var response = await _httpClient.GetAsync($"/api/civic/contests/{contestId}/candidates");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Candidate>>(content, _jsonOptions) ?? new();
        }

        /// <summary>
        /// Gets a specific candidate by ID.
        /// </summary>
        public async Task<Candidate?> GetCandidateAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/civic/candidates/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Candidate>(content, _jsonOptions);
        }

        /// <summary>
        /// Gets all favorited candidates for an election.
        /// </summary>
        public async Task<List<FavoriteCandidate>> GetFavoritesAsync(int electionId)
        {
            var response = await _httpClient.GetAsync($"/api/elections/{electionId}/favorites");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<FavoriteCandidate>>(content, _jsonOptions) ?? new();
        }

        /// <summary>
        /// Sets a candidate as favorite for a contest.
        /// </summary>
        public async Task<bool> SetFavoriteAsync(int contestId, int candidateId)
        {
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/favorites/{contestId}/{candidateId}", content);
            response.EnsureSuccessStatusCode();
            return true;
        }

        /// <summary>
        /// Removes the favorite candidate for a contest.
        /// </summary>
        public async Task<bool> RemoveFavoriteAsync(int contestId)
        {
            var response = await _httpClient.DeleteAsync($"/api/favorites/{contestId}");
            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<List<AddressSuggestion>> GetAddressSuggestionsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return new List<AddressSuggestion>();

            var response = await _httpClient.GetAsync($"/api/places/autocomplete?query={Uri.EscapeDataString(query)}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<AddressSuggestion>>(content, _jsonOptions) ?? new();
        }

        public async Task<UserAddress?> GetPlaceDetailsAsync(string placeId)
        {
            var response = await _httpClient.GetAsync($"/api/places/details/{placeId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserAddress>(content, _jsonOptions);
        }
    }
}
