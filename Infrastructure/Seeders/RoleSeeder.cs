using Domain.Entities.Roles.Constants;
using Domain.Entities.Roles.Models;
using Loop.PersonalLogger;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Seeders;

public static class RoleSeeder
{

    public static async Task SeedRolesAsync(AppDbContext context)
    {
        var roles = new List<Role>
        {
            new() { Name = RoleConstants.Admin,  CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Name = RoleConstants.Client, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        var existingRoleNames = await context.Roles
            .Select(r => r.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase);

        var rolesToInsert = roles
            .Where(r => !existingRoleNames.Contains(r.Name))
            .ToList();

        if (rolesToInsert.Any())
        {
            context.Roles.AddRange(rolesToInsert);
            await context.SaveChangesAsync();
            PersonalLogger.Log($"Se agregaron {rolesToInsert.Count} roles: {string.Join(", ", rolesToInsert.Select(r => r.Name))}.");
        }
        else
        {
            PersonalLogger.Log("Todos los roles ya existen. No se realizaron cambios.");
        }
    }
}
