using clinicautp.ViewModels;

namespace clinicautp.Views
{
    public partial class MedicamentoActualizarPage : ContentPage
    {
        public MedicamentoActualizarPage(MedicamentoActualizarViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            // Llamar al m�todo LoadMedicamentoAsync despu�s de establecer el BindingContext
            _ = LoadMedicamentoDataAsync(viewModel);
        }

        private async Task LoadMedicamentoDataAsync(MedicamentoActualizarViewModel viewModel)
        {
            // Aqu� puedes pasar el c�digo del medicamento que quieres cargar
            await viewModel.LoadMedicamentoAsync(viewModel.CodMedicamento);
        }
    }
}
