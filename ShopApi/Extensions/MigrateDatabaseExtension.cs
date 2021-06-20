using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ShopApi.Extensions
{
    public static class MigrateDatabaseExtension
    {
        public static IHost MigrateDatabase<T>(this IHost webHost) where T : DbContext
        {
            using var scope = webHost.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                var db = services.GetRequiredService<T>();
                // db.Database.EnsureDeleted();
                // db.Database.EnsureCreated();
                db.Database.Migrate();

                logger.LogInformation("Migration Succeeded");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database.");
            }

            return webHost;
        }
    }
}
