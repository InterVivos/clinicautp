using System.Collections.ObjectModel;
using System.Threading.Tasks;
using clinicautp.Models;
using clinicautp.Utilities;
using clinicautp.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using clinicautp.Views;

namespace clinicautp.ViewModels
{
    public partial class MedicamentoMainViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

        // Propiedad que almacena la lista de medicamentos y notifica cambios en la vista
        [ObservableProperty]
        private ObservableCollection<Medicamento> listaMedicamentos = new ObservableCollection<Medicamento>();

        // Constructor que inicializa el ViewModel con el contexto de base de datos
        // Constructor que inicializa el ViewModel con el contexto de base de datos
        public MedicamentoMainViewModel(ClinicaDBContext context)
        {
            _dbContext = context;
            // Cargar la lista de medicamentos al inicializar el ViewModel
            MainThread.BeginInvokeOnMainThread(async () => await LoadMedicamentos());
        }

        // Método para cargar los medicamentos desde la base de datos
        public async Task LoadMedicamentos()
        {
            // Limpiar la lista antes de agregar nuevos datos
            ListaMedicamentos.Clear();

            // Obtener todos los medicamentos desde la base de datos
            var lista = await _dbContext.Medicamentos.ToListAsync();

            if (lista.Any())
            {
                foreach (var medicamento in lista)
                {
                    ListaMedicamentos.Add(medicamento);
                }
            }
        }

        // Comando para actualizar los datos de un medicamento
        [RelayCommand]
        private async Task ActualizarMedicamento(string codMedicamento)
        {
            // Lógica para cargar el medicamento a actualizar
            var medicamento = await _dbContext.Medicamentos
                .FirstOrDefaultAsync(m => m.CodMedicamento == codMedicamento);

            if (medicamento != null)
            {
                //Navegar a la página de edición o actualización del medicamento
                var viewModel = new MedicamentoActualizarViewModel(_dbContext, codMedicamento);
                var page = new MedicamentoActualizarPage(viewModel);
                await Shell.Current.Navigation.PushAsync(page);
            }
        }

        // Comando para eliminar un medicamento
        [RelayCommand]
        private async Task EliminarMedicamento(string codMedicamento)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "¿Eliminar medicamento?", "Sí", "No");
            if (answer)
            {
                var medicamento = await _dbContext.Medicamentos
                    .FirstOrDefaultAsync(m => m.CodMedicamento == codMedicamento);

                if (medicamento != null)
                {
                    _dbContext.Medicamentos.Remove(medicamento);
                    await _dbContext.SaveChangesAsync();

                    // Volver a cargar la lista de medicamentos
                    LoadMedicamentos();
                }
            }
        }

        // Comando para agregar un nuevo medicamento
        [RelayCommand]
        private async Task AgregarMedicamento()
        {
            var viewModel = new MedicamentoRegisterViewModel(_dbContext);
            var page = new MedicamentoRegisterPage(viewModel);
            await Shell.Current.Navigation.PushAsync(page);
        }

        // Comando para ver los detalles de un medicamento
        [RelayCommand]
        private async Task VerDetalleMedicamento(string codMedicamento)
        {
            // Lógica para ver detalles del medicamento
            var medicamento = await _dbContext.Medicamentos
                .FirstOrDefaultAsync(m => m.CodMedicamento == codMedicamento);

            if (medicamento != null)
            {
                //var viewModel = new MedicamentoDetailViewModel(_dbContext);
                // var page = new MedicamentoDetailPage(viewModel);
                //await Shell.Current.Navigation.PushAsync(page);
            }
        }
    }
}