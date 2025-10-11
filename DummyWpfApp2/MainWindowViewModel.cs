using System.Collections.ObjectModel;
using MediatR;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PR.Web.Application.Smurfs;

namespace DummyWpfApp2;

public class MainWindowViewModel : ViewModelBase
{
    private string _buttonText;
    private readonly IMediator _mediator;

    public RelayCommand LoadSmurfsCommand { get; }

    public string ButtonText
    {
        get => _buttonText;
        set
        {
            _buttonText = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<SmurfDto> Smurfs { get; } = new();

    public MainWindowViewModel(
        IMediator mediator)
    {
        _buttonText = "Populate list with smurfs";
        _mediator = mediator;

        LoadSmurfsCommand = new RelayCommand(async () => await LoadSmurfsAsync());
    }

    private async Task LoadSmurfsAsync()
    {
        var smurfs = await _mediator.Send(new List.Query());

        Smurfs.Clear();
        foreach (var smurfDto in smurfs.Value)
        {
            Smurfs.Add(smurfDto);
        }
    }
}