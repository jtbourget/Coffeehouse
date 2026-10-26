using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Coffeehouse.Models;

public class CandidateProfileDisplay
{
    public int Id { get; set; }
    public string OcdId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? WebsiteUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsVerified { get; set; }
    public List<Video> Videos { get; set; } = new();

    public string FormattedStatus => IsVerified ? "Verified" : "Pending Verification";
    public string Subtitle => string.IsNullOrWhiteSpace(Bio) ? OcdId : Bio;
}

public class SaveCandidateProfileDto
{
    public string OcdId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? WebsiteUrl { get; set; }
}

public class ProblemDetailsDto
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public int? Status { get; set; }
    public string? Detail { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}

public class AuthResult
{
    public bool IsSuccess { get; private init; }
    public string? ErrorMessage { get; private init; }

    public static AuthResult Success() => new() { IsSuccess = true };
    public static AuthResult Failed(string message) => new() { IsSuccess = false, ErrorMessage = message };
}

public class ApiResult
{
    public bool IsSuccess { get; init; }
    public HttpStatusCode StatusCode { get; init; }
    public ProblemDetailsDto? Problem { get; init; }
    public string? NetworkErrorMessage { get; init; }

    public bool IsUnauthorized => StatusCode == HttpStatusCode.Unauthorized;
    public bool IsNotFound => StatusCode == HttpStatusCode.NotFound;
    public bool IsValidationError => StatusCode == HttpStatusCode.BadRequest;
    public bool IsConflict => StatusCode == HttpStatusCode.Conflict;

    public string ErrorMessage
    {
        get
        {
            if (!string.IsNullOrEmpty(NetworkErrorMessage)) return NetworkErrorMessage;
            if (Problem?.Detail != null) return Problem.Detail;
            if (Problem?.Title != null) return Problem.Title;
            if (IsUnauthorized) return "Authentication required. Please log in.";
            if (IsNotFound) return "Record not found.";
            return $"Request failed with status {(int)StatusCode}";
        }
    }

    public static ApiResult Ok() => new() { IsSuccess = true, StatusCode = HttpStatusCode.OK };
    public static ApiResult Created() => new() { IsSuccess = true, StatusCode = HttpStatusCode.Created };
    public static ApiResult NoContent() => new() { IsSuccess = true, StatusCode = HttpStatusCode.NoContent };

    public static ApiResult Failure(HttpStatusCode code, ProblemDetailsDto? problem = null, string? networkError = null) =>
        new() { IsSuccess = false, StatusCode = code, Problem = problem, NetworkErrorMessage = networkError };
}

public class ApiResult<T> : ApiResult
{
    public T? Value { get; init; }

    public static ApiResult<T> Success(T value, HttpStatusCode code = HttpStatusCode.OK) =>
        new() { IsSuccess = true, StatusCode = code, Value = value };

    public static new ApiResult<T> Failure(HttpStatusCode code, ProblemDetailsDto? problem = null, string? networkError = null) =>
        new() { IsSuccess = false, StatusCode = code, Problem = problem, NetworkErrorMessage = networkError };
}
