using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MediatR;
using PR.Persistence;
using PR.Persistence.EntityFrameworkCore;
using PR.Web.Application.Interfaces;
using PR.Web.Application.Core;
using PR.Web.Application.Smurfs;
using PR.Web.Infrastructure.Pagination;

namespace DummyConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Load connection string from appsettings.json or environment
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                    connectionString = "Data source=babuska2.db";

                    services.AddAppDataPersistence<PRDbContextBase>(options =>
                        options.UseSqlite(connectionString));

                    services.AddAutoMapper(assemblies: typeof(MappingProfiles).Assembly);
                    services.AddApplication();   // registers MediatR and handlers
                    services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
                    services.AddScoped<IPagingHandler<SmurfDto>, PagingHandler<SmurfDto>>();
 
                    services.AddMediatR(cfg =>
                        cfg.RegisterServicesFromAssemblyContaining<List.Query>());
                })
                .Build();

            using var scope = host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PRDbContextBase>();
            await db.Database.MigrateAsync();
            await Seeding.SeedDatabase(db);

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(new List.Query{Params = new SmurfParams()});

            Console.WriteLine($"\nSmurfs:");
            foreach (var smurf in result.Value)
            {
                Console.WriteLine($" {smurf.Name}");
            }

            Console.WriteLine("\nDone");
        }
    }
}
