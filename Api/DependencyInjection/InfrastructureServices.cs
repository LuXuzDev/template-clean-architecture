using Domain.RefreshTokens.Repository;
using Domain.Roles.Repository;
using Domain.Users.Repository;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace Api.DependencyInjection;

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices 
       (this IServiceCollection services, IConfiguration configuration)
    {
        #region Base de datos
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                 b => b.MigrationsAssembly("Infrastructure"))
            );
        #endregion


        #region Repositories

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        #endregion


        return services;
    }

    /// <summary>
    /// Ejecuta el seeder de base de datos para inicializar datos base como roles y usuarios.
    /// </summary>
    /// <param name="app">Instancia de la aplicación.</param>
    /// <returns>Una tarea asincrónica que representa el proceso de seeding.</returns>
    public static async Task UseDatabaseSeederAsync(this WebApplication app)
    {
        await DatabaseSeeder.SeedAsync(app.Services);
    }
}

