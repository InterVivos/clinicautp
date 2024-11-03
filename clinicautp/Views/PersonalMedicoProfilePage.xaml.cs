using clinicautp.ViewModels;

namespace clinicautp.Views
{
    public partial class PersonalMedicoProfilePage : ContentPage
    {
        public PersonalMedicoProfilePage(PersonalMedicoProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            // Llamar al m�todo LoadPersonalMedicoAsync despu�s de establecer el BindingContext
            _ = LoadPersonalMedicoDataAsync(viewModel);
        }

        private async Task LoadPersonalMedicoDataAsync(PersonalMedicoProfileViewModel viewModel)
        {
            // Cargar los datos del personal m�dico
            await viewModel.LoadPersonalMedicoAsync(); // Aseg�rate de que este m�todo existe en el ViewModel
        }
    }
}
