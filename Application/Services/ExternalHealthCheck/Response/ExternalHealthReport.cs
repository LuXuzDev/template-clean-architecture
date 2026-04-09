using Application.Services.ExternalHealthCheck.Enums;

namespace Application.Services.ExternalHealthCheck.Response;

public class ExternalHealthReport
{
    /// <summary>
    /// Lista de todos los servicios revisados
    /// </summary>
    public List<ExternalHealthResponse> Services { get; set; } = new();

    /// <summary>
    /// Estado global calculado a partir de todos los servicios
    /// </summary>
    public ExternalServiceStatus Status { get; set; } = ExternalServiceStatus.Healthy;
}