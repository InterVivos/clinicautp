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
    public partial class AdminViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

   
        [ObservableProperty]
        private ObservableCollection<string> listaCedulasPacientes = new ObservableCollection<string>();

     
        public AdminViewModel(ClinicaDBContext context)
        {
            _dbContext = context;
            LoadPacientes();
        }

        public async void LoadPacientes()
        {
            ListaCedulasPacientes.Clear();

            var lista = await _dbContext.Pacientes
                .Select(p => p.Cedula)
                .ToListAsync();

            foreach (var cedula in lista)
            {
                ListaCedulasPacientes.Add(cedula);
            }
        }

        [RelayCommand]
        private async Task ActualizarPaciente(string cedula)
        {
            var profileViewModel = new ProfileViewModel(_dbContext);
            AppState.Instance.CedulaPaciente = cedula;
            await profileViewModel.LoadPacienteAsync();

            var uri = $"{nameof(ProfilePage)}?cedula={cedula}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task EliminarPaciente(string cedula)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "¿Eliminar paciente?", "Sí", "No");
            if (answer)
            {
                var paciente = await _dbContext.Pacientes
                    .FirstOrDefaultAsync(p => p.Cedula == cedula);

                if (paciente != null)
                {
                    _dbContext.Pacientes.Remove(paciente);
                    await _dbContext.SaveChangesAsync();

                    LoadPacientes();
                }
            }
        }

        [RelayCommand]
        private async Task VerHistorialMedico(string cedula)
        {
            AppState.Instance.CedulaPaciente = cedula;
            var uri = $"{nameof(MainViewPage)}?cedula={cedula}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task AgregarPacienteNuevo()
        {
            var viewModel = new AdminRegisterPacienteViewModel(_dbContext);
            var page = new AdminRegisterPacientePage(viewModel);
            await Shell.Current.Navigation.PushAsync(page);
        }

        [RelayCommand]
        private async Task NavegarPersonalMedico()
        {
            await Shell.Current.GoToAsync(nameof(AdminPersonalMedicoMainPage));
        }

        [RelayCommand]
        private async Task NavegarMedicamentos()
        {
            await Shell.Current.GoToAsync(nameof(MedicamentoMainPage));
        }

    }
}
