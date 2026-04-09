using Application.Services.ExternalHealthCheck.Enums;
using Shared.Results.Errors;


namespace Application.Services.ExternalHealthCheck.Response;

public class ExternalHealthResponse
{
    public string Name { get; set; } = default!;
    public bool IsCritical { get; set; }
    public ExternalServiceStatus Status { get; set; }
    public Error? Error { get; set; }
}