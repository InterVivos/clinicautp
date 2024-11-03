using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using clinicautp.DataAccess;
using clinicautp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using clinicautp.DTOs;

namespace clinicautp.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {

        private readonly ClinicaDBContext _dbContext;
        public RegisterViewModel(ClinicaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [ObservableProperty]
        private PacienteDTO pacienteDto = new PacienteDTO();

        [ObservableProperty]
        private string cedula;

        [ObservableProperty]
        private string contrasena;

        [RelayCommand]
        private async Task Register()
        {
            try
            {
                // Verificar si la cédula o la contraseña son "admin"
                if (cedula.ToLower() == "admin" && contrasena.ToLower() == "admin")
                {
                    await Shell.Current.DisplayAlert("Error", "No es posible registrar un administrador con la cédula y contraseña 'admin'.", "OK");
                    return;
                }

                // Verificar si el paciente ya existe en la base de datos
                var pacienteExistente = await _dbContext.Pacientes
                    .FirstOrDefaultAsync(p => p.Cedula == cedula);

                if (pacienteExistente == null)
                {
                    // Si el paciente no existe, crear uno nuevo
                    var nuevoPaciente = new Paciente
                    {
                        Cedula = cedula,
                        Contrasena = contrasena
                    };

                    _dbContext.Pacientes.Add(nuevoPaciente);
                    await _dbContext.SaveChangesAsync();

                    // Navegar a MainPage después del registro exitoso
                    await Shell.Current.GoToAsync(nameof(MainPage));
                }
                else
                {
                    // Mostrar mensaje de error si el paciente ya está registrado
                    await Shell.Current.DisplayAlert("Error", "El paciente ya está registrado.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si ocurre una excepción
                await Shell.Current.DisplayAlert("Error", $"Se produjo un error: {ex.Message}", "OK");
            }
        }
    }
}
