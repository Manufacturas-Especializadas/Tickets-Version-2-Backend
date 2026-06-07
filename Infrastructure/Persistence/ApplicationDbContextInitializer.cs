using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    public static class ApplicationDbContextInitializer
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");

            try
            {
                if (!await context.Roles.AnyAsync())
                {
                    logger.LogInformation("Sembrando Roles por defecto...");
                    context.Roles.AddRange(
                        new Role { Name = "Admin" },
                        new Role { Name = "Sistemas" },
                        new Role { Name = "Usuario" }
                    );
                    await context.SaveChangesAsync(default);
                }

                var usersToMigrate = await context.Users
                    .Where(u => u.PasswordHash.StartsWith("AQAAAA"))
                    .ToListAsync();

                if (usersToMigrate.Any())
                {
                    logger.LogInformation($"Migrando contraseñas de {usersToMigrate.Count} usuarios a BCrypt...");

                    foreach (var user in usersToMigrate)
                    {
                        string newPassword = user.PayRollNumber.ToString();

                        user.PasswordHash = passwordHasher.Hash(newPassword);

                        user.RolId = 1;
                    }

                    await context.SaveChangesAsync(default);
                    logger.LogInformation("Migración de contraseñas completada exitosamente.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocurrió un error al sembrar o migrar la base de datos.");
                throw;
            }
        }
    }
}