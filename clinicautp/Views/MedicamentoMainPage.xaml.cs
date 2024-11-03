using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class MedicamentoMainPage : ContentPage
{
    public MedicamentoMainPage(MedicamentoMainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Cargar los medicamentos cuando la página aparezca
        await ((MedicamentoMainViewModel)BindingContext).LoadMedicamentos();
    }
}
