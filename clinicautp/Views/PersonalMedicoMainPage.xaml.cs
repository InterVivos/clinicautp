using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class PersonalMedicoMainPage : ContentPage
{
	public PersonalMedicoMainPage(PersonalMedicoMainViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}