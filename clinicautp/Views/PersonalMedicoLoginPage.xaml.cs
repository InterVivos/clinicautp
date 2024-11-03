using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class PersonalMedicoLoginPage : ContentPage
{
	public PersonalMedicoLoginPage(PersonalMedicoLoginViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}