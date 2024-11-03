using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class LogInPage : ContentPage
{
	public LogInPage(LogInViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}