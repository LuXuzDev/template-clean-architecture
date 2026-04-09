using Domain.Exceptions;
using Loop.PersonalLogger;

namespace Shared.Exception;

/// <summary>
/// Proporciona métodos auxiliares estáticos para lanzar excepciones personalizadas
/// de forma consistente en toda la aplicación, según la categoría y el contexto del error.
/// </summary>
public static class ExceptionHelper
{
    #region Errores de infraestructura

    /// <summary>
    /// Lanza una excepción relacionada con fallos en la base de datos.
    /// </summary>
    /// <param name="details">Detalles del error (ej. "Error al guardar el negocio").</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.Database"/>.</exception>
    public static void ThrowDatabase(string details)
    {
        throw new ApiException(ExceptionType.Database, "Error en la base de datos", details);
    }

    /// <summary>
    /// Lanza una excepción cuando falla la comunicación con un servicio externo.
    /// </summary>
    /// <param name="details">Detalles del fallo (ej. "Error al procesar pago con pasarela externa").</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.ExternalService"/>.</exception>
    public static void ThrowExternalService(string details)
    {
        throw new ApiException(ExceptionType.ExternalService, "Error en servicio externo", details);
    }

    /// <summary>
    /// Lanza una excepción por problemas de red o conectividad.
    /// </summary>
    /// <param name="details">Detalles del problema de red.</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.Network"/>.</exception>
    public static void ThrowNetwork(string details)
    {
        throw new ApiException(ExceptionType.Network, "Error de red o conectividad", details);
    }

    #endregion


    #region Errores internos del sistema

    /// <summary>
    /// Lanza una excepción para errores inesperados o no manejados.
    /// </summary>
    /// <param name="details">Detalles del error (ideal para logs).</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.Unexpected"/>.</exception>
    public static void ThrowUnexpected(string details)
    {
        throw new ApiException(ExceptionType.Unexpected,"Error inesperado" ,details);
    }

    /// <summary>
    /// Lanza una excepción cuando hay un problema en la configuración de la aplicación.
    /// </summary>
    /// <param name="details">Detalles del error de configuración (ej. "Variable de entorno faltante").</param>
    /// <exception cref="ApiException">Con <see cref="ExceptionType.Configuration"/>.</exception>
    public static void ThrowConfiguration(string details)
    {
        throw new ApiException(ExceptionType.Configuration, "Error de configuración del sistema", details);
    }

    #endregion
}
