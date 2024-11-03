using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class CitaMainViewPage : ContentPage
{
    private readonly CitaMainViewModel _viewModel;

    public CitaMainViewPage(CitaMainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Recargar los datos para asegurar que se muestren las actualizaciones
        await _viewModel.ObtenerCitas();
    }
}
