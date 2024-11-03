using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class AdminPersonalMedicoMainPage : ContentPage
{
    public AdminPersonalMedicoMainPage(AdminPersonalMedicoMainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Aquí puedes agregar lógica adicional si es necesario
        await ((AdminPersonalMedicoMainViewModel)BindingContext).LoadPersonalMedico();
    }
}
