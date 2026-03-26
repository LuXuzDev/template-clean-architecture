namespace Domain.Exceptions;

public enum ExceptionType
{

    #region Errores de infraestructura 

    /// <summary>
    /// Error en la base de datos: fallo en consultas o integridad.
    /// Equivale a HTTP 500 (Internal Server Error).
    /// </summary>
    Database = 500,

    /// <summary>
    /// Fallo al consumir o comunicarse con un servicio externo.
    /// Equivale a HTTP 502 (Bad Gateway).
    /// </summary>
    ExternalService = 502,

    /// <summary>
    /// Error de red o tiempo de espera.
    /// Equivale a HTTP 504 (Gateway Timeout).
    /// </summary>
    Network = 504,
    #endregion


    #region Errores internos del sistema 

    /// <summary>
    /// Error inesperado o no controlado.
    /// Equivale a HTTP 500 (Internal Server Error).
    /// </summary>
    Unexpected = 500,

    /// <summary>
    /// Error de configuración: parámetros faltantes o erróneos.
    /// Equivale a HTTP 500 (Internal Server Error).
    /// </summary>
    Configuration = 500
    #endregion
}
