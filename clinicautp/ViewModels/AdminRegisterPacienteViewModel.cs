using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using clinicautp.DataAccess;
using clinicautp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace clinicautp.ViewModels
{
    public partial class AdminRegisterPacienteViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

        // Propiedades para datos del paciente
        [ObservableProperty]
        private string contrasena;

        [ObservableProperty]
        private string cedula;

        [ObservableProperty]
        private string nombre;

        [ObservableProperty]
        private string apellido;

        [ObservableProperty]
        private DateTime fechaNacimiento;

        [ObservableProperty]
        private string sangre;

        [ObservableProperty]
        private string correo;

        [ObservableProperty]
        private bool esDonador;

        public AdminRegisterPacienteViewModel(ClinicaDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [RelayCommand]
        private async Task AgregarPaciente()
        {
            // Validar que todos los campos estén completos
            if (string.IsNullOrWhiteSpace(Cedula) ||
                string.IsNullOrWhiteSpace(Contrasena) ||
                string.IsNullOrWhiteSpace(Nombre) ||
                string.IsNullOrWhiteSpace(Apellido) ||
                string.IsNullOrWhiteSpace(Sangre) ||
                string.IsNullOrWhiteSpace(Correo))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, complete todos los campos obligatorios.", "OK");
                return;
            }

            // Verificar si el paciente ya existe
            var existePaciente = await _dbContext.Pacientes.AnyAsync(p => p.Cedula == Cedula);
            if (existePaciente)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El paciente ya está registrado.", "OK");
                return;
            }

            // Crear un nuevo paciente
            var nuevoPaciente = new Paciente
            {
                Contrasena = Contrasena,
                Cedula = Cedula,
                Nombre = Nombre,
                Apellido = Apellido,
                FechaNacimiento = FechaNacimiento,
                Sangre = Sangre,
                Correo = Correo,
                EsDonador = EsDonador
            };

            await _dbContext.Pacientes.AddAsync(nuevoPaciente);
            await _dbContext.SaveChangesAsync();

            await Application.Current.MainPage.DisplayAlert("Éxito", "Paciente agregado correctamente.", "OK");

            
            ClearFields();
        }

        
        private void ClearFields()
        {
            Cedula = string.Empty;
            Contrasena = string.Empty;
            Nombre = string.Empty;
            Apellido = string.Empty;
            FechaNacimiento = DateTime.MinValue;
            Sangre = string.Empty;
            Correo = string.Empty;
            EsDonador = false;
        }
    }
}
