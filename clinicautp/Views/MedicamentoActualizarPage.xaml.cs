using clinicautp.ViewModels;

namespace clinicautp.Views
{
    public partial class MedicamentoActualizarPage : ContentPage
    {
        public MedicamentoActualizarPage(MedicamentoActualizarViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            // Llamar al método LoadMedicamentoAsync después de establecer el BindingContext
            _ = LoadMedicamentoDataAsync(viewModel);
        }

        private async Task LoadMedicamentoDataAsync(MedicamentoActualizarViewModel viewModel)
        {
            // Aquí puedes pasar el código del medicamento que quieres cargar
            await viewModel.LoadMedicamentoAsync(viewModel.CodMedicamento);
        }
    }
}
