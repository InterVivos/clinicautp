using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using clinicautp.DataAccess;
using clinicautp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using clinicautp.Views;
using clinicautp.Utilities;

namespace clinicautp.ViewModels
{
    public partial class LogInViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

        public LogInViewModel(ClinicaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Propiedades para la Cédula y la Contraseña
        [ObservableProperty]
        private string cedula;

        [ObservableProperty]
        private string contrasena;

        // Comando para el proceso de Login
        [RelayCommand]
        private async Task Login()
        {
            if (AppState.Instance.EsAdmin(cedula, contrasena))
            {
                // Lógica para el acceso de administrador: navegar a la página de administración
                await Shell.Current.GoToAsync(nameof(AdminPage));
            }

            else
            {
                var paciente = await _dbContext.Pacientes.FirstOrDefaultAsync(p => p.Cedula == cedula && p.Contrasena == contrasena);

                if (paciente != null)
                {
                    // Lógica para el acceso correcto del paciente: navegar a la página de historial médico
                    AppState.Instance.CedulaPaciente = paciente.Cedula;
                    await Shell.Current.GoToAsync(nameof(MainViewPage));
                }
                else
                {
                    // Mostrar mensaje de error de credenciales incorrectas
                    await Shell.Current.DisplayAlert("Error", "Cédula o contraseña incorrecta.", "OK");
                }
            }
        }
    }
}