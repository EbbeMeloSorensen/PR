using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace DummyWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //// From ChatGPT
        //public static IHost AppHost { get; private set; }

        //// From ChatGPT
        //public App()
        //{
        //    AppHost = Host.CreateDefaultBuilder()
        //        .ConfigureServices((context, services) =>
        //        {
        //            // Register your Application layer and WPF services
        //            //services.AddApplicationLayer();
        //            services.AddSingleton<MainWindowViewModel>();
        //            services.AddSingleton<MainWindow>();
        //        })
        //        .Build();
        //}

        //// From ChatGPT
        //protected override async void OnStartup(
        //    StartupEventArgs e)
        //{
        //    await AppHost.StartAsync();

        //    var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
        //    mainWindow.Show();

        //    base.OnStartup(e);
        //}

        //// From ChatGPT
        //protected override async void OnExit(
        //    ExitEventArgs e)
        //{
        //    await AppHost.StopAsync();
        //    AppHost.Dispose();
        //    base.OnExit(e);
        //}
    }
}
