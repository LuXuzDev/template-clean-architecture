using Application.Services.ExternalHealthCheck.Enums;
using Application.Services.ExternalHealthCheck.Interface;
using Application.Services.ExternalHealthCheck.Response;
using Loop.PersonalLogger;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Shared.Results;
using Shared.Results.Errors.ExternalServices.PostgresSQL;


namespace Application.Services.ExternalHealthCheck.Implementations;

public class PostgresHealthCheck : IExternalHealthCheck
{
    public string Name => "PostgreSQL";
    public bool IsCritical => true;

    private readonly string _connectionString;

    public PostgresHealthCheck(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection")!;
    }

    public async Task<Result<ExternalHealthResponse>> CheckAsync(CancellationToken ct)
    {
        PersonalLogger.Log($"Comenzando revision del servicio {Name}...");

        var externalHealthCheck = new ExternalHealthResponse
        {
            Name = Name,
            IsCritical = IsCritical,
            Status = ExternalServiceStatus.Healthy
        };

        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            PersonalLogger.Log($"Error: Cadena de conexion no configurada para el servicio {Name}.", LogType.Error);
            return Result<ExternalHealthResponse>.Failure(PostgresError.NotConfigured);
        }


        NpgsqlConnectionStringBuilder builder;
        try
        {
            builder = new NpgsqlConnectionStringBuilder(_connectionString);
        }
        catch (Exception ex)
        {
            PersonalLogger.Log($"Error: Cadena de conexion invalida para el servicio {Name}. Detalles: {ex.Message}", LogType.Error);
            return Result<ExternalHealthResponse>.Failure(PostgresError.NotConfigured);
        }

        if (string.IsNullOrWhiteSpace(builder.Host))
        {
            PersonalLogger.Log($"Error: Host de la base de datos no configurado para el servicio {Name}.", LogType.Error);
            return Result<ExternalHealthResponse>.Failure(PostgresError.PostgresHostMissing);
        }

        if (string.IsNullOrWhiteSpace(builder.Database))
        {
            PersonalLogger.Log($"Error: Nombre de la base de datos no configurado para el servicio {Name}.", LogType.Error);
            return Result<ExternalHealthResponse>.Failure(PostgresError.PostgresDatabaseMissing);
        }


        // Validar conexión real
        try
        {
            await using var conn = new NpgsqlConnection(builder.ConnectionString);
            await conn.OpenAsync(ct);

            await using var cmd = new NpgsqlCommand("SELECT 1", conn)
            {
                CommandTimeout = 2
            };

            await cmd.ExecuteScalarAsync(ct);

            return Result<ExternalHealthResponse>.Success(externalHealthCheck);
        }
        catch (TimeoutException)
        {
            PersonalLogger.Log($"Error: Tiempo de espera agotado al intentar conectar con el servicio {Name}.", LogType.Error);
            return Result<ExternalHealthResponse>.Failure(PostgresError.PostgresTimeout);
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == "3D000") // database does not exist
            {
                PersonalLogger.Log("Base de Datos no existe aún, migración la creará. Ignorando error crítico.", LogType.Warning);
                return Result<ExternalHealthResponse>.Success(new ExternalHealthResponse
                {
                    Name = Name,
                    IsCritical = IsCritical,
                    Status = ExternalServiceStatus.Healthy
                });
            }

            return Result<ExternalHealthResponse>.Failure(PostgresError.PostgresConnectionFailed);
        }
        catch (Exception ex)
        {

            PersonalLogger.Log($"Error: No se pudo conectar con el servicio {Name}. Detalles: {ex.Message}", LogType.Error);
            return Result<ExternalHealthResponse>.Failure(PostgresError.PostgresConnectionFailed);
        }
    }
}