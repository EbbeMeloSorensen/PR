using GalaSoft.MvvmLight;

namespace DummyWpfApp2;

public class MainWindowViewModel : ViewModelBase
{
    private string _greeting;

    public string Greeting
    {
        get => _greeting;
        set
        {
            _greeting = value;
            RaisePropertyChanged();
        }
    }

    public MainWindowViewModel()
    {
        _greeting = "Hello from MainWindowViewModel";
    }
}