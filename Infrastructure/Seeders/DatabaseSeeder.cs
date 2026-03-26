using Application.Services.Blob;
using Application.Services.DataProtector;
using Domain.Entities.Users.Models;
using LuxuzDev.PersonalLogger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
        var dataProtectorFactory = scope.ServiceProvider.GetRequiredService<IDataProtectorFactory>();
        var webHost = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        var blobService = scope.ServiceProvider.GetRequiredService<IBlobService>();

        PersonalLogger.Log("🔄 Aplicando migraciones de base de datos...", LogType.Info);
        await context.Database.MigrateAsync();
        PersonalLogger.Log("✅ Migraciones aplicadas correctamente.", LogType.Success);

        PersonalLogger.Log("🌱 Iniciando seeding de base de datos...", LogType.Info);

        await RoleSeeder.SeedRolesAsync(context);
        await UserSeeder.SeedUsersAsync(context, passwordService, dataProtectorFactory);

        PersonalLogger.Log("✅ Seeding completado correctamente.", LogType.Success);
    }
}
