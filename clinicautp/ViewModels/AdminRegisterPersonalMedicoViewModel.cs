using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using clinicautp.DataAccess;
using clinicautp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.ObjectModel;

namespace clinicautp.ViewModels
{
    public partial class AdminRegisterPersonalMedicoViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

        // Propiedades para el nuevo personal médico
        [ObservableProperty]
        private string cedula;

        [ObservableProperty]
        private string contrasena;

        [ObservableProperty]
        private string cargo;

        /*[ObservableProperty]
        private Especialidad especialidad;*/
        [ObservableProperty]
        ObservableCollection<string> listaEspecialidades = new ObservableCollection<string>();

        [ObservableProperty]
        private string especialidadSeleccionada;

        [ObservableProperty]
        private string nombre;

        [ObservableProperty]
        private string apellido;

        [ObservableProperty]
        private string correo;

        [ObservableProperty]
        private string telefono;

        // Constructor
        public AdminRegisterPersonalMedicoViewModel(ClinicaDBContext context)
        {
            _dbContext = context;
        }

    
        [RelayCommand]
        private async Task AgregarPersonalMedico()
        {
            if (string.IsNullOrWhiteSpace(cedula) || string.IsNullOrWhiteSpace(contrasena))
            {
                await Shell.Current.DisplayAlert("Error", "Cédula y contraseña son obligatorios.", "OK");
                return;
            }

            try
            {
                
                var nuevoMedico = new PersonalMedico
                {
                    Cedula = Cedula,
                    Contrasena = Contrasena,
                    Cargo = Cargo,
                    EspecialidadNombre = EspecialidadSeleccionada,
                    Nombre = Nombre,
                    Apellido = Apellido,
                    Correo = Correo,
                    Telefono = Telefono
                };

                // Agregar el nuevo médico a la base de datos
                _dbContext.PersonalMedicos.Add(nuevoMedico);
                await _dbContext.SaveChangesAsync();

                // Limpiar los campos
                ClearFields();

                await Shell.Current.DisplayAlert("Éxito", "Personal médico agregado correctamente.", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Ocurrió un error al agregar: {ex.Message}", "OK");
            }
        }

    
        private void ClearFields()
        {
            Cedula = string.Empty;
            Contrasena = string.Empty;
            Cargo = string.Empty;
            //Especialidad = string.Empty;
            EspecialidadSeleccionada = null;
            Nombre = string.Empty;
            Apellido = string.Empty;
            Correo = string.Empty;
            Telefono = string.Empty;
        }

        public async Task LoadEspecialidades()
        {
            await Especialidad.LoadEspecialidades(_dbContext, listaEspecialidades);
        }
    }
}
