using Shared.Results.Errors;


namespace Shared.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }
    public IReadOnlyList<Error>? Errors { get; }

    protected Result(bool isSuccess, Error? error, IReadOnlyList<Error>? errors)
    {
        if (isSuccess && (error != null || (errors != null && errors.Any())))
            throw new ArgumentException("Success result cannot have errors");

        if (!isSuccess && error == null && (errors == null || !errors.Any()))
            throw new ArgumentException("Failure result must have at least one error");

        IsSuccess = isSuccess;
        Error = error;
        Errors = errors;
    }

    public static Result Success()
        => new(true, null, null);

    public static Result Failure(Error error)
        => new(false, error, null);

    public static Result Failure(IEnumerable<Error> errors)
        => new(false, null, errors.ToList());
}


public class Result<T> : Result
{
    public T? Value { get; }

    private Result(T value) : base(true, null, null)
    {
        Value = value;
    }

    private Result(Error error) : base(false, error, null) { }

    private Result(IEnumerable<Error> errors) : base(false, null, errors.ToList()) { }

    public static Result<T> Success(T value)
        => new(value);

    public static new Result<T> Failure(Error error)
        => new(error);

    public static new Result<T> Failure(IEnumerable<Error> errors)
        => new(errors.ToList());
}