using Microsoft.Extensions.DependencyInjection;
using StructureMap;

namespace DummyWpfApp
{
    public class BootStrapper
    {
        // From ChatGPT
        private readonly IServiceProvider _services;

        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                try
                {
                    var mainWindowViewModel = Container.For<MainWindowViewModelRegistry>().GetInstance<MainWindowViewModel>();

                    return mainWindowViewModel;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unknown error: {e.Message}");
                    throw;
                }
            }
        }

        // From ChatGPT
        public BootStrapper()
        {
            //_services = App.AppHost.Services;
        }

        // From ChatGPT
        public void Initialize()
        {
            var mainWindow = _services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
