using clinicautp.ViewModels;

namespace clinicautp.Views
{
    public partial class PersonalMedicoProfilePage : ContentPage
    {
        public PersonalMedicoProfilePage(PersonalMedicoProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            // Llamar al método LoadPersonalMedicoAsync después de establecer el BindingContext
            _ = LoadPersonalMedicoDataAsync(viewModel);
        }

        private async Task LoadPersonalMedicoDataAsync(PersonalMedicoProfileViewModel viewModel)
        {
            // Cargar los datos del personal médico
            await viewModel.LoadPersonalMedicoAsync(); // Asegúrate de que este método existe en el ViewModel
        }
    }
}
