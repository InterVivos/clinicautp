using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using clinicautp.DataAccess;
using clinicautp.Models;
using clinicautp.DTOs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using clinicautp.Utilities;
using clinicautp.Views;

namespace clinicautp.ViewModels
{
    public partial class PersonalMedicoMainViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

        // Propiedad que almacena la lista de personal médico y notifica cambios en la vista
        [ObservableProperty]
        private ObservableCollection<PersonalMedicoDTO> listaPersonalMedico = new ObservableCollection<PersonalMedicoDTO>();

        [ObservableProperty]
        private ObservableCollection<CitaDTO> listaCitas = new ObservableCollection<CitaDTO>();

        // Constructor que inicializa el ViewModel con el contexto de base de datos
        public PersonalMedicoMainViewModel(ClinicaDBContext context)
        {
            _dbContext = context;

            // Ejecuta el método Obtener en el hilo principal de la interfaz de usuario
            //MainThread.BeginInvokeOnMainThread(new Action(async () => await ObtenerPersonalMedico()));

            MainThread.BeginInvokeOnMainThread(new Action(async () => await ObtenerCitas()));
        }

        public async Task ObtenerCitas()
        {
            ListaCitas.Clear();



            var lista = await _dbContext.Citas
                .Where(c => c.Especialidad == _dbContext.PersonalMedicos.Find(AppState.Instance.CedulaPersonalMedico).EspecialidadNombre)
                .Where(c => c.Estado != "Completada")
                .ToListAsync();

            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    ListaCitas.Add(new CitaDTO
                    {
                        Id = item.Id,
                        FechaCita = item.FechaCita,
                        Especialidad = item.Especialidad,
                        FechaCreacion = item.FechaCreacion,
                        CedulaPaciente = item.CedulaPaciente,
                        Estado = item.Estado,
                        Observaciones = item.Observaciones,
                        HoraCita = item.HoraCita
                    });
                }
            }
        }


        // Comando para cerrar sesión
        [RelayCommand]
        private async Task LogOut()
        {
            try
            {
                // Limpiar cualquier información de sesión
                AppState.Instance.CedulaPersonalMedico = null; // Limpiar la cédula del personal médico

                // Redirigir al usuario a la página de login
                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo cerrar sesión: {ex.Message}", "OK");
            }
        }

        // Comando para navegar a la página de perfil
        [RelayCommand]
        private async Task NavegarPerfil(string cedula)
        {
            // Suponiendo que la cédula se obtiene desde el estado de la aplicación o algún otro lugar
            var ced = AppState.Instance.CedulaPersonalMedico;
            var uri = $"{nameof(PersonalMedicoProfilePage)}?cedula={ced}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        public async Task NavegarHorarioMedico()
        {
            // Navegar a la página de horario médico
            await Shell.Current.GoToAsync(nameof(HorarioMedicoPage1));
        }

        [RelayCommand]
        public async Task NavegarCrearPDF()
        {
            await Shell.Current.GoToAsync(nameof(ReferenciaEspecialidadPage));
        }
    }
}
