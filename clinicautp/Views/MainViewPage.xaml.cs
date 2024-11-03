using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class MainViewPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainViewPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // Método llamado cada vez que la página se muestra
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Recargar los datos para asegurar que se muestren las actualizaciones
        await _viewModel.ObtenerHistorialMedico();
    }
}