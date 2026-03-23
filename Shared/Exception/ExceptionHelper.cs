using Domain.Exceptions;

namespace Shared.Exception;

/// <summary>
/// Proporciona métodos auxiliares estáticos para lanzar excepciones personalizadas
/// de forma consistente en toda la aplicación, según la categoría y el contexto del error.
/// </summary>
public static class ExceptionHelper
{
    #region Errores de dominio

    /// <summary>
    /// Lanza una excepción indicando que un recurso solicitado no fue encontrado.
    /// </summary>
    /// <param name="details">Descripción detallada del error (ej. "Negocio con ID 123 no encontrado").</param>
    /// <param name="errorCode">Código de error personalizado opcional para identificación específica.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.NotFound"/>.</exception>
    public static void ThrowNotFound(string details, ExceptionCode errorCode = ExceptionCode.None)
    {
        throw new ApiException(ExceptionType.NotFound, errorCode, "Recurso no encontrado", details);
    }

    /// <summary>
    /// Lanza una excepción por violación de reglas de validación de entrada.
    /// </summary>
    /// <param name="details">Detalles del error de validación (ej. "El correo electrónico no es válido").</param>
    /// <param name="errorCode">Código de error personalizado opcional.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.Validation"/>.</exception>
    public static void ThrowValidation(string details, ExceptionCode errorCode = ExceptionCode.None)
    {
        throw new ApiException(ExceptionType.Validation, errorCode, "Error de validación", details);
    }

    /// <summary>
    /// Lanza una excepción cuando se intenta realizar una operación que genera un conflicto de estado.
    /// </summary>
    /// <param name="details">Detalles del conflicto (ej. "El negocio ya está en favoritos").</param>
    /// <param name="errorCode">Código de error personalizado opcional.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.Conflict"/>.</exception>
    public static void ThrowConflict(string details, ExceptionCode errorCode = ExceptionCode.None)
    {
        throw new ApiException(ExceptionType.Conflict, errorCode, "Recurso en conflicto", details);
    }

    /// <summary>
    /// Lanza una excepción cuando se viola una regla de negocio específica del dominio.
    /// </summary>
    /// <param name="details">Explicación de la regla incumplida (ej. "No puede crear más de 3 negocios").</param>
    /// <param name="errorCode">Código de error personalizado opcional.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.BusinessRule"/>.</exception>
    public static void ThrowBusinessRule(string details, ExceptionCode errorCode = ExceptionCode.None)
    {
        throw new ApiException(ExceptionType.BusinessRule, errorCode, "Regla de negocio incumplida", details);
    }

    #endregion


    #region Errores de autenticación/autorización

    /// <summary>
    /// Lanza una excepción cuando el usuario no ha proporcionado credenciales válidas.
    /// </summary>
    /// <param name="details">Detalles del error de autenticación.</param>
    /// <param name="errorCode">Código de error personalizado opcional.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.Unauthorized"/>.</exception>
    public static void ThrowUnauthorized(string details, ExceptionCode errorCode = ExceptionCode.None)
    {
        throw new ApiException(ExceptionType.Unauthorized, errorCode, "No autenticado", details);
    }

    /// <summary>
    /// Lanza una excepción cuando el usuario autenticado no tiene permisos para realizar la acción solicitada.
    /// </summary>
    /// <param name="details">Detalles del acceso prohibido (ej. "Solo el propietario puede editar este negocio").</param>
    /// <param name="errorCode">Código de error personalizado opcional.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.Forbidden"/>.</exception>
    public static void ThrowForbidden(string details, ExceptionCode errorCode = ExceptionCode.None)
    {
      throw new ApiException(ExceptionType.Forbidden, errorCode, "Acceso prohibido", details);
    }

    /// <summary>
    /// Lanza una excepción cuando el token de autenticación ha expirado.
    /// </summary>
    /// <param name="details">Detalles adicionales (opcional).</param>
    /// <param name="errorCode">Código de error personalizado opcional.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.TokenExpired"/>.</exception>
    public static void ThrowTokenExpired(string details, ExceptionCode errorCode = ExceptionCode.None)
    {
        throw new ApiException(ExceptionType.TokenExpired, errorCode, "Token o sesión expirada", details);
    }

    #endregion


    #region Errores de infraestructura

    /// <summary>
    /// Lanza una excepción relacionada con fallos en la base de datos.
    /// </summary>
    /// <param name="details">Detalles del error (ej. "Error al guardar el negocio").</param>
    /// <param name="errorCode">Código de error personalizado opcional.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.Database"/>.</exception>
    public static void ThrowDatabase(string details, ExceptionCode errorCode = ExceptionCode.None)
    {
        throw new ApiException(ExceptionType.Database, errorCode, "Error en la base de datos", details);
    }

    /// <summary>
    /// Lanza una excepción cuando falla la comunicación con un servicio externo.
    /// </summary>
    /// <param name="details">Detalles del fallo (ej. "Error al procesar pago con pasarela externa").</param>
    /// <param name="errorCode">Código de error personalizado opcional.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.ExternalService"/>.</exception>
    public static void ThrowExternalService(string details, ExceptionCode errorCode = ExceptionCode.None)
    {
        throw new ApiException(ExceptionType.ExternalService, errorCode, "Error en servicio externo", details);
    }

    /// <summary>
    /// Lanza una excepción por problemas de red o conectividad.
    /// </summary>
    /// <param name="details">Detalles del problema de red.</param>
    /// <param name="errorCode">Código de error personalizado opcional.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.Network"/>.</exception>
    public static void ThrowNetwork(string details, ExceptionCode errorCode = ExceptionCode.None)
    {
        throw new ApiException(ExceptionType.Network, errorCode, "Error de red o conectividad", details);
    }

    #endregion


    #region Errores internos del sistema

    /// <summary>
    /// Lanza una excepción para errores inesperados o no manejados.
    /// </summary>
    /// <param name="details">Detalles del error (ideal para logs).</param>
    /// <param name="errorCode">Código de error personalizado opcional.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.Unexpected"/>.</exception>
    public static void ThrowUnexpected(string details, ExceptionCode errorCode = ExceptionCode.None)
    {
        throw new ApiException(ExceptionType.Unexpected, errorCode, "Error inesperado", details);
    }

    /// <summary>
    /// Lanza una excepción cuando hay un problema en la configuración de la aplicación.
    /// </summary>
    /// <param name="details">Detalles del error de configuración (ej. "Variable de entorno faltante").</param>
    /// <param name="errorCode">Código de error personalizado opcional.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.Configuration"/>.</exception>
    public static void ThrowConfiguration(string details, ExceptionCode errorCode = ExceptionCode.None)
    {
        throw new ApiException(ExceptionType.Configuration, errorCode, "Error de configuración del sistema", details);
    }

    #endregion
}
