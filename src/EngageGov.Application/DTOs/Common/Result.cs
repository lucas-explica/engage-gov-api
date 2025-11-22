namespace EngageGov.Application.DTOs.Common;

/// <summary>
/// Generic result wrapper for operations
/// Implements Result pattern for better error handling
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string? ErrorMessage { get; private set; }
    public IEnumerable<string>? ValidationErrors { get; private set; }

    private Result(bool isSuccess, T? data, string? errorMessage, IEnumerable<string>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
        ValidationErrors = validationErrors;
    }

    public static Result<T> Success(T data) => new(true, data, null);

    public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);

    public static Result<T> ValidationFailure(IEnumerable<string> validationErrors) 
        => new(false, default, "Validation failed", validationErrors);
}

/// <summary>
/// Non-generic result for operations without return value
/// </summary>
public class Result
{
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public IEnumerable<string>? ValidationErrors { get; private set; }

    private Result(bool isSuccess, string? errorMessage, IEnumerable<string>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ValidationErrors = validationErrors;
    }

    public static Result Success() => new(true, null);

    public static Result Failure(string errorMessage) => new(false, errorMessage);

    public static Result ValidationFailure(IEnumerable<string> validationErrors) 
        => new(false, "Validation failed", validationErrors);
}
