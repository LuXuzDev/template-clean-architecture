namespace Domain.Exceptions;
public class ApiException : Exception
{
    public ExceptionType ExceptionType { get; set; }
    public ExceptionCode ExceptionCode { get; set; } 
    public string ErrorMessage { get; set; }
    public string ErrorDetails { get; set; } = string.Empty;

    public ApiException
        (ExceptionType exceptionType,
         ExceptionCode exceptionCode,
         string errorMessage,
         string errorDetails) : base(errorMessage)
    {
        ExceptionType = exceptionType;
        ExceptionCode = exceptionCode;
        ErrorMessage = errorMessage;
        ErrorDetails = errorDetails;
    }
}

