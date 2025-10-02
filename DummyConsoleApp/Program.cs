using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
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
                    connectionString = "Data source=babuska.db";

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
            db.Database.Migrate();  // Applies any pending migrations

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var result = mediator.Send(new List.Query());

            Console.WriteLine("So far so good");
        }
    }
}


//namespace PR.Web.Application
//{
//    public static class ServiceCollectionExtensions
//    {
//        public static IServiceCollection AddApplication(
//            this IServiceCollection services,
//            string connectionString = null)
//        {
//            // Register MediatR
//            services.AddMediatR(cfg =>
//                cfg.RegisterServicesFromAssemblyContaining<ServiceCollectionExtensions>());

//            // Register Persistence dependencies
//            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

//            // If a connection string is provided, register EF Core
//            if (!string.IsNullOrEmpty(connectionString))
//            {
//                services.AddDbContextFactory<PRDbContextBase>(options =>
//                    options.UseSqlite(connectionString));
//            }

//            return services;
//        }
//    }
//}