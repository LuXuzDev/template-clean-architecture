using Shared.Results.Errors;


namespace Shared.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public IReadOnlyList<Error> Errors { get; }

    protected Result(bool isSuccess, IEnumerable<Error>? errors)
    {
        var list = errors?.ToList() ?? new List<Error>();

        if (isSuccess && list.Count > 0)
            throw new ArgumentException("Success result cannot have errors");

        if (!isSuccess && list.Count == 0)
            throw new ArgumentException("Failure result must have at least one error");

        IsSuccess = isSuccess;
        Errors = list;
    }

    public static Result Success()
        => new(true, null);

    public static Result Failure(Error error)
        => new(false, new[] { error });

    public static Result Failure(IEnumerable<Error> errors)
        => new(false, errors);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(T value) : base(true, null)
    {
        Value = value;
    }

    private Result(Error error) : base(false, new List<Error> { error }) { }

    private Result(IEnumerable<Error> errors) : base(false, errors.ToList()) { }

    public static Result<T> Success(T value)
        => new(value);

    public static new Result<T> Failure(Error error)
        => new(error);

    public static new Result<T> Failure(IEnumerable<Error> errors)
        => new(errors.ToList());
}