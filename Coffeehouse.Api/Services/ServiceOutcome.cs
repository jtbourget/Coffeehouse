namespace Coffeehouse.Api.Services;

/// <summary>
/// Status of a service operation outcome.
/// Represents business outcomes independent of HTTP status codes.
/// </summary>
public enum ServiceOutcomeStatus
{
    Success,
    Duplicate,
    NotFound,
    InvalidInput
}

/// <summary>
/// Represents the outcome of a business service operation.
/// Reports results, validation, and domain errors to controllers as business outcomes rather than HTTP status codes.
/// </summary>
/// <typeparam name="T">The type of data returned on success.</typeparam>
public class ServiceOutcome<T>
{
    public ServiceOutcomeStatus Status { get; init; }
    public T? Data { get; init; }
    public string? ErrorMessage { get; init; }

    public bool IsSuccess => Status == ServiceOutcomeStatus.Success;

    public static ServiceOutcome<T> Success(T data) => new()
    {
        Status = ServiceOutcomeStatus.Success,
        Data = data
    };

    public static ServiceOutcome<T> Duplicate(string message) => new()
    {
        Status = ServiceOutcomeStatus.Duplicate,
        ErrorMessage = message
    };

    public static ServiceOutcome<T> NotFound(string message) => new()
    {
        Status = ServiceOutcomeStatus.NotFound,
        ErrorMessage = message
    };

    public static ServiceOutcome<T> InvalidInput(string message) => new()
    {
        Status = ServiceOutcomeStatus.InvalidInput,
        ErrorMessage = message
    };
}
