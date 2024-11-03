using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class PacienteCitaPage : ContentPage
{
	public PacienteCitaPage(PacienteCitaViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}