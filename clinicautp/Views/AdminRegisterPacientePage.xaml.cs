using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class AdminRegisterPacientePage : ContentPage
{
	public AdminRegisterPacientePage(AdminRegisterPacienteViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}