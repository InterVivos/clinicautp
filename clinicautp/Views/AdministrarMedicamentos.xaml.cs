using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class AdministrarMedicamentos : ContentPage
{
    public AdministrarMedicamentos(HistorialMedicoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}