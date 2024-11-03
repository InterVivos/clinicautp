using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class AdmRegisterPersonalMedicoPage : ContentPage
{
	public AdmRegisterPersonalMedicoPage(AdminRegisterPersonalMedicoViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}