using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.Logging;
using PR.Persistence;
using PR.Persistence.EntityFrameworkCore;
using PR.Web.Application.Interfaces;
using PR.Web.Application.Core;
using PR.Web.Application.Smurfs;
using PR.Web.Infrastructure.Pagination;

namespace DummyWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost Host { get; private set; }

        protected override void OnStartup(
            StartupEventArgs e)
        {
            base.OnStartup(e);

            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var connectionString = "Data source=babuska3.db";

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

            //using var scope = Host.Services.CreateScope();
            //var db = scope.ServiceProvider.GetRequiredService<PRDbContextBase>();
            //await db.Database.MigrateAsync();
            //await Seeding.SeedDatabase(db);

            Host.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Host.Dispose();
            base.OnExit(e);
        }
    }
}
