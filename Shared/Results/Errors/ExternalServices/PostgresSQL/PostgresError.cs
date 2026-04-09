namespace Shared.Results.Errors.ExternalServices.PostgresSQL;

public static class PostgresError
{
    public static readonly Error NotConfigured =
        new("POSTGRES_CONFIG_INVALID", "PostgreSQL connection string is missing or invalid. Please check configuration.",500);

    public static readonly Error PostgresHostMissing =
        new("POSTGRES_CONFIG_HOST_MISSING", "The PostgreSQL host is missing in the connection string. Please check the configuration.",500);

    public static readonly Error PostgresDatabaseMissing =
        new("POSTGRES_CONFIG_DATABASE_MISSING", "The PostgreSQL database name is missing in the connection string. Please check the configuration.",500);

    public static readonly Error PostgresTimeout =
        new("POSTGRES_TIMEOUT","Timeout while attempting to connect to PostgreSQL. The service may be slow or unresponsive.",504 );

    public static readonly Error PostgresConnectionFailed =
        new("POSTGRES_CONNECTION_FAILED","Unable to connect to PostgreSQL. Please check if the service is running and network settings are correct.",500);
}
