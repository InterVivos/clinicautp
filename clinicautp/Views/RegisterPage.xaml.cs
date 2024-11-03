using clinicautp.ViewModels;
using clinicautp.DataAccess;

namespace clinicautp.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
    }
}