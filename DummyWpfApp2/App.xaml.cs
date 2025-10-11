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

            var splash = new SplashScreenWindow();
            splash.Show();

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    //var connectionString = "Data source=babuska27.db";
                    var connectionString = "Server=localhost;Port=5432;User Id=root;Password=root;Database=DB_DummyWpfApp";

                    services.AddAppDataPersistence<PRDbContextBase>(options =>
                    {
                        //options.UseSqlite(connectionString);
                        options.UseNpgsql(connectionString);
                    });

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

            Task.Run(async () =>
            {
                using var scope = _host.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<PRDbContextBase>();
                db.Database.MigrateAsync().GetAwaiter().GetResult();
                Seeding.SeedDatabase(db).GetAwaiter().GetResult();

                await Current.Dispatcher.InvokeAsync(() =>
                {
                    var scope = _host!.Services.CreateScope();
                    var mainWindow = scope.ServiceProvider.GetRequiredService<MainWindow>();
                    splash.Close();
                    mainWindow.Show();
                });
            });
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
