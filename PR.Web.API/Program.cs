using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PR.Web.API;
using PR.Web.Persistence;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Original from project template
            //CreateHostBuilder(args).Build().Run();

            // Changes made by instructor (for creating the database from the migration)
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await context.Database.MigrateAsync();
                await Seed.SeedData(context, userManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}