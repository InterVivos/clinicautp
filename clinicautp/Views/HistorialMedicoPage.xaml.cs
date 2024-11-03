using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class HistorialMedicoPage : ContentPage
{
	public HistorialMedicoPage(HistorialMedicoViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}