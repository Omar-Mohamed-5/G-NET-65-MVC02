using G_NET_65_MVC02.DAL.Data.DataSeeding;
using G_NET_65_MVC02.DAL.Data.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace G_NET_65_MVC02_PL
{
    public static class ProgramExtensions
    {
        public static async Task MigrateAndSeedDataAsync(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var dbContext = Scope.ServiceProvider.GetRequiredService<GymDBContext>();
            var Logger = Scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

            if (PendingMigrations.Any())
            {
                Logger.LogInformation($"Applying {PendingMigrations.Count()} Pending Migrations");
                await dbContext.Database.MigrateAsync();
            }

            var SeedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbContext, SeedFolderPath, Logger);
        }
    }
}
