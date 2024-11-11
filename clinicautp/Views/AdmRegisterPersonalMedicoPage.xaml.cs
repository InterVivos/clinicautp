using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class AdmRegisterPersonalMedicoPage : ContentPage
{
	public AdmRegisterPersonalMedicoPage(AdminRegisterPersonalMedicoViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

	protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Aqu� puedes agregar l�gica adicional si es necesario
        await ((AdminRegisterPersonalMedicoViewModel)BindingContext).LoadEspecialidades();
    }
}