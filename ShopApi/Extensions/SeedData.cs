using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Database.Data
{
    public static class SeedData
    {
        public static IHost MigrateDatabase<T>(this IHost webHost) where T : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            return webHost;
        }

        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services
                .GetRequiredService<RoleManager<IdentityRole>>();
          

            var userManager = services
                .GetRequiredService<UserManager<IdentityUser>>();
            await EnsureTestDoctorRoleAsync(userManager);
        }

        private static async Task EnsureTestDoctorRoleAsync(UserManager<IdentityUser> userManager)
        {
            var testAdmin = await userManager.Users
                .Where(x => x.Email == "nelu.tataru@med.com")
                .SingleOrDefaultAsync();

        }

        //private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        //{

        //}
    }
}
