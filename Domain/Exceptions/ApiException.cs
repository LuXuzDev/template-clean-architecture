namespace Domain.Exceptions;
public class ApiException : Exception
{
    public ExceptionType ExceptionType { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorDetails { get; set; } = string.Empty;

    public ApiException
        (ExceptionType exceptionType,
         string errorMessage,
         string errorDetails) : base(errorMessage)
    {
        ExceptionType = exceptionType;
        ErrorMessage = errorMessage;
        ErrorDetails = errorDetails;
    }
}

