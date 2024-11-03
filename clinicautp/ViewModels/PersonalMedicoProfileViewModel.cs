using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using clinicautp.DataAccess;
using clinicautp.Models;
using Microsoft.EntityFrameworkCore;
using clinicautp.Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace clinicautp.ViewModels
{
    public partial class PersonalMedicoProfileViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

        [ObservableProperty]
        private PersonalMedico _personalMedico;

        [ObservableProperty]
        private string _contrasena;

        [ObservableProperty]
        private string _nombre;

        [ObservableProperty]
        private string _apellido;

        [ObservableProperty]
        private string _telefono;

        [ObservableProperty]
        private string _correo;

        [ObservableProperty]
        private string _cargo;

        [ObservableProperty]
        private string _especialidad;

        public PersonalMedicoProfileViewModel(ClinicaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadPersonalMedicoAsync()
        {
            // Obtener la cédula del personal médico desde el estado de la aplicación
            string cedula = AppState.Instance.CedulaPersonalMedico; // Asegúrate de que este estado esté definido

            PersonalMedico = await _dbContext.PersonalMedicos.FirstOrDefaultAsync(pm => pm.Cedula == cedula);
            if (PersonalMedico != null)
            {
                // Inicializar los valores con los datos del personal médico
                Contrasena = PersonalMedico.Contrasena;
                Nombre = PersonalMedico.Nombre;
                Apellido = PersonalMedico.Apellido;
                Telefono = PersonalMedico.Telefono;
                Correo = PersonalMedico.Correo;
                Cargo = PersonalMedico.Cargo;
                Especialidad = PersonalMedico.Especialidad;
            }
            else
            {
                // Mensaje de error si el personal médico no se encuentra
                await Application.Current.MainPage.DisplayAlert("Error", "Personal médico no encontrado.", "OK");
            }
        }

        [RelayCommand]
        private async Task ActualizarDatos()
        {
            if (PersonalMedico == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Personal médico no encontrado.", "OK");
                return;
            }

            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Apellido) || string.IsNullOrWhiteSpace(Correo) || string.IsNullOrWhiteSpace(Cargo))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, complete todos los campos obligatorios.", "OK");
                return;
            }

            // Actualizar los datos del personal médico
            PersonalMedico.Contrasena = Contrasena;  // Actualiza la contraseña si es necesario
            PersonalMedico.Nombre = Nombre;
            PersonalMedico.Apellido = Apellido;
            PersonalMedico.Telefono = Telefono;
            PersonalMedico.Correo = Correo;
            PersonalMedico.Cargo = Cargo;
            PersonalMedico.Especialidad = Especialidad;

            // Guardar los cambios en la base de datos
            _dbContext.PersonalMedicos.Update(PersonalMedico);
            await _dbContext.SaveChangesAsync();

            // Mostrar mensaje de éxito
            await Application.Current.MainPage.DisplayAlert("Éxito", "Los datos se han actualizado correctamente.", "OK");
        }
    }
}
