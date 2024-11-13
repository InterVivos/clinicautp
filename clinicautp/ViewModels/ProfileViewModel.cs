using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using clinicautp.DataAccess;
using clinicautp.DTOs;
using clinicautp.Views;
using clinicautp.Models;
using Microsoft.EntityFrameworkCore;
using clinicautp.Utilities;

namespace clinicautp.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

        [ObservableProperty]
        private Paciente _paciente;

        [ObservableProperty]
        private string _contrasena;

        [ObservableProperty]
        private string _nombre;

        [ObservableProperty]
        private string _apellido;

        [ObservableProperty]
        private string _telefono;

        [ObservableProperty]
        private string _sangre;

        [ObservableProperty]
        private DateTime _fechaNacimiento;

        [ObservableProperty]
        private string _correo;

        [ObservableProperty]
        private bool _esDonador;

        public ProfileViewModel(ClinicaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadPacienteAsync()
        {
            // Obtener la cédula del paciente desde el estado de la aplicación
            string cedula = AppState.Instance.CedulaPaciente;

            Paciente = await _dbContext.Pacientes.FirstOrDefaultAsync(p => p.Cedula == cedula);
            if (Paciente != null)
            {
                // Inicializar los valores con los datos del paciente
                Contrasena = Paciente.Contrasena;
                Nombre = Paciente.Nombre;
                Apellido = Paciente.Apellido;
                Telefono = Paciente.Telefono;
                Correo = Paciente.Correo;
                Sangre = Paciente.Sangre;
                FechaNacimiento = Paciente.FechaNacimiento;
                EsDonador = Paciente.EsDonador;
            }
            else
            {
                // Mensaje de error si el paciente no se encuentra
                await Application.Current.MainPage.DisplayAlert("Error", "Paciente no encontrado.", "OK");
            }
        }



        [RelayCommand]
        private async Task ActualizarDatos()
        {
            if (Paciente == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Paciente no encontrado.", "OK");
                return;
            }

            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Apellido) || string.IsNullOrWhiteSpace(Correo))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, complete todos los campos obligatorios.", "OK");
                return;
            }

            // Actualizar los datos del paciente
            Paciente.Contrasena = Contrasena;  // Actualiza la contraseña si es necesario
            Paciente.Nombre = Nombre;
            Paciente.Apellido = Apellido;
            Paciente.Telefono = Telefono;
            Paciente.Sangre = Sangre;
            Paciente.FechaNacimiento = FechaNacimiento;
            Paciente.Correo = Correo;
            Paciente.EsDonador = EsDonador;

            // Guardar los cambios en la base de datos
            _dbContext.Pacientes.Update(Paciente);
            await _dbContext.SaveChangesAsync();

            // Mostrar mensaje de éxito
            await Application.Current.MainPage.DisplayAlert("Éxito", "Los datos se han actualizado correctamente.", "OK");
        }
    }
}
