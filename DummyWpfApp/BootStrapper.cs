using StructureMap;

namespace DummyWpfApp
{
    public class BootStrapper
    {
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
    }
}
