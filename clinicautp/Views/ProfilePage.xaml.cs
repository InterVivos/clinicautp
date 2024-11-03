using clinicautp.DataAccess;
using clinicautp.ViewModels;
using clinicautp.Models;

namespace clinicautp.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(ProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            // Llamar al método LoadPacienteAsync después de establecer el BindingContext
            _ = LoadPatientDataAsync(viewModel);
        }

        private async Task LoadPatientDataAsync(ProfileViewModel viewModel)
        {
            // Cargar los datos del paciente
            await viewModel.LoadPacienteAsync();
        }
    }
}
