using Avalonia.Controls;
using AvaloniaFrontend.ViewModels;

namespace AvaloniaFrontend;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(); // simple, direct binding
    }
}