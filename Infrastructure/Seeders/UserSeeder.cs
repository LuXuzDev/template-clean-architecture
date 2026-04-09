using Application.Helpers.Hasher;
using Application.Services.DataProtector;
using Domain.Entities.Roles.Constants;
using Domain.Entities.Users.Models;
using Loop.PersonalLogger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Seeders;

public static class UserSeeder
{
    public static async Task SeedUsersAsync(
        AppDbContext context,
        IPasswordHasher<User> passwordService,
        IDataProtectorFactory dataProtectorFactory)
    {
        var emailProtector = dataProtectorFactory.Create(DataPorpuse.UserEmail);

        var existingEmailHashes = await context.Users
            .Select(u => u.HashedEmail)
            .ToHashSetAsync();

        var existingRoleNames = await context.Roles
            .ToDictionaryAsync(r => r.Name, r => r.Id);

        var usersToInsert = new List<User>();

        var baseUsers = new List<(string Email, string Password, string RoleName)>
        {
            ("admin@gmail.com", "Admin123!", RoleConstants.Admin),
            ("client@gmail.com", "Cliene123!", RoleConstants.Client)
        };

        foreach (var (emailRaw, password, roleName) in baseUsers)
        {
            var email = emailRaw.Trim().ToLowerInvariant();
            var emailHash = HasherHelper.Hash(email);

            if (!existingEmailHashes.Contains(emailHash) &&
                existingRoleNames.TryGetValue(roleName, out var roleId))
            {
                var user = new User
                {
                    EncryptedEmail = emailProtector.Protect(email),
                    HashedEmail = emailHash,

                    Password = password,

                    RoleId = roleId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                // 🔐 Hash password
                user.Password = passwordService.HashPassword(user, password);

                usersToInsert.Add(user);
            }
        }

        if (usersToInsert.Any())
        {
            context.Users.AddRange(usersToInsert);
            await context.SaveChangesAsync();

            PersonalLogger.Log(
                $"Se agregaron {usersToInsert.Count} usuarios: " +
                string.Join(", ", usersToInsert.Select(u => emailProtector.Unprotect(u.EncryptedEmail)))
            );
        }
        else
        {
            PersonalLogger.Log("Todos los usuarios base ya existen.");
        }
    }
}