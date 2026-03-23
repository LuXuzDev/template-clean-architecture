namespace Domain.Exceptions;

public enum ExceptionType
{
    #region Errores de dominio 

    /// <summary>
    /// Error de validación: los datos de entrada o entidad no son válidos.
    /// Equivale a HTTP 400 (Bad Request).
    /// </summary>
    Validation = 400,

    /// <summary>
    /// Recurso no encontrado: el elemento solicitado (usuario, restaurante, plato, etc.) no existe.
    /// Equivale a HTTP 404 (Not Found).
    /// </summary>
    NotFound = 404,

    /// <summary>
    /// Conflicto: colisión de estado o elemento duplicado.
    /// Equivale a HTTP 409 (Conflict).
    /// </summary>
    Conflict = 409,

    /// <summary>
    /// Incumplimiento de una regla de negocio.
    /// Equivale a HTTP 422 (Unprocessable Entity).
    /// </summary>
    BusinessRule = 422,
    #endregion


    #region Errores de autenticación/autorización 

    /// <summary>
    /// Error de autenticación: usuario no autenticado o sesión inválida.
    /// Equivale a HTTP 401 (Unauthorized).
    /// </summary>
    Unauthorized = 401,

    /// <summary>
    /// Error de autorización: usuario autenticado sin permisos suficientes.
    /// Equivale a HTTP 403 (Forbidden).
    /// </summary>
    Forbidden = 403,

    /// <summary>
    /// Token o sesión expirada: requiere nueva autenticación.
    /// </summary>
    TokenExpired = 401,
    #endregion


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
