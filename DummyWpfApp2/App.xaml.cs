using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using PR.Persistence;
using PR.Persistence.EntityFrameworkCore;
using PR.Web.Application.Core;
using PR.Web.Application.Interfaces;
using PR.Web.Application.Smurfs;
using PR.Web.Infrastructure.Pagination;

namespace DummyWpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var connectionString = "Data source=babuska27.db";
                    services.AddAppDataPersistence<PRDbContextBase>(options =>
                        //options.UseSqlite(connectionString));
                        options.UseSqlite(connectionString));

            services.AddAutoMapper(assemblies: typeof(MappingProfiles).Assembly);
                    services.AddApplication();   // registers MediatR and handlers
                    services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
                    services.AddScoped<IPagingHandler<SmurfDto>, PagingHandler<SmurfDto>>();

                    services.AddMediatR(cfg =>
                        cfg.RegisterServicesFromAssemblyContaining<List.Query>());

                    // Register our ViewModel and View
                    services.AddSingleton<MainWindowViewModel>();
                    services.AddTransient<MainWindow>();
                })
                .Build();

            using var scope = _host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PRDbContextBase>();
            //await db.Database.MigrateAsync();
            //await Seeding.SeedDatabase(db);
            db.Database.MigrateAsync().GetAwaiter().GetResult();
            Seeding.SeedDatabase(db).GetAwaiter().GetResult();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host is not null)
            {
                await _host.StopAsync();
            }

            _host?.Dispose();
            base.OnExit(e);
        }
    }
}
