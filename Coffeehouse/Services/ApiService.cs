using System.Text.Json;
using System.Text;
using Coffeehouse.Models;
using System.Diagnostics;

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

        public ApiService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5032") };
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        /// <summary>
        /// Gets the user's saved address.
        /// </summary>
        public async Task<UserAddress?> GetAddressAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/address");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                    {
                        return JsonSerializer.Deserialize<UserAddress>(content, _jsonOptions);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching address: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Saves or updates the user's address.
        /// </summary>
        public async Task<bool> SaveAddressAsync(UserAddress address)
        {
            try
            {
                var json = JsonSerializer.Serialize(address, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/address", content);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"API returned: {response.StatusCode}");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving address: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets a list of all elections.
        /// </summary>
        public async Task<List<Election>> GetElectionsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/elections");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Election>>(content, _jsonOptions) ?? new();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching elections: {ex.Message}");
            }
            return new List<Election>();
        }

        /// <summary>
        /// Gets a specific election by ID.
        /// </summary>
        public async Task<Election?> GetElectionAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/elections/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Election>(content, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching election {id}: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Gets contests for a given election.
        /// </summary>
        public async Task<List<Contest>> GetContestsAsync(int electionId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/elections/{electionId}/contests");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Contest>>(content, _jsonOptions) ?? new();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching contests for election {electionId}: {ex.Message}");
            }
            return new List<Contest>();
        }

        /// <summary>
        /// Gets candidates for a given contest.
        /// </summary>
        public async Task<List<Candidate>> GetCandidatesAsync(int contestId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/contests/{contestId}/candidates");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Candidate>>(content, _jsonOptions) ?? new();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching candidates for contest {contestId}: {ex.Message}");
            }
            return new List<Candidate>();
        }

        /// <summary>
        /// Gets a specific candidate by ID.
        /// </summary>
        public async Task<Candidate?> GetCandidateAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/candidates/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Candidate>(content, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching candidate {id}: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Gets all favorited candidates for an election.
        /// </summary>
        public async Task<List<FavoriteCandidate>> GetFavoritesAsync(int electionId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/elections/{electionId}/favorites");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<FavoriteCandidate>>(content, _jsonOptions) ?? new();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching favorites for election {electionId}: {ex.Message}");
            }
            return new List<FavoriteCandidate>();
        }

        /// <summary>
        /// Sets a candidate as favorite for a contest.
        /// </summary>
        public async Task<bool> SetFavoriteAsync(int contestId, int candidateId)
        {
            try
            {
                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/favorites/{contestId}/{candidateId}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error setting favorite {candidateId} for contest {contestId}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Removes the favorite candidate for a contest.
        /// </summary>
        public async Task<bool> RemoveFavoriteAsync(int contestId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/favorites/{contestId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error removing favorite for contest {contestId}: {ex.Message}");
                return false;
            }
        }
    }
}
