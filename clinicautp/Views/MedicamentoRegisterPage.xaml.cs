using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class MedicamentoRegisterPage : ContentPage
{
	public MedicamentoRegisterPage(MedicamentoRegisterViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}