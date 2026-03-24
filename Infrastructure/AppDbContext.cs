using Domain.RefreshTokens.Models;
using Domain.Roles.Models;
using Domain.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="AppDbContext"/> con las opciones especificadas.
    /// </summary>
    /// <param name="options">Opciones de configuración para el contexto de base de datos.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    #region Entities

    /// <summary>
    /// Conjunto de usuarios en la base de datos.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Conjunto de roles en la base de datos.
    /// </summary>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Conjunto de token de refresco en la base de datos.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    /// <summary>
    /// Conjunto de tokens invalidos en la base de datos.
    /// </summary>
    public DbSet<TokenBlackList> TokenBlackList { get; set; }

    #endregion
}
