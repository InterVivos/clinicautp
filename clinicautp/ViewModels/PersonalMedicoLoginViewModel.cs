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
    public partial class PersonalMedicoLoginViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

        public PersonalMedicoLoginViewModel(ClinicaDBContext dbContext)
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
                // Buscar el personal médico en la base de datos
                var personalMedico = await _dbContext.PersonalMedicos
                    .FirstOrDefaultAsync(pm => pm.Cedula == cedula && pm.Contrasena == contrasena);

                if (personalMedico != null)
                {
                    // Lógica para el acceso correcto del personal médico: navegar a la página principal de personal médico
                    AppState.Instance.CedulaPersonalMedico = personalMedico.Cedula;
                    await Shell.Current.GoToAsync(nameof(PersonalMedicoMainPage));
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
