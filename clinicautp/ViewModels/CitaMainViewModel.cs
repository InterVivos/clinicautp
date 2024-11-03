using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using clinicautp.DataAccess;
using clinicautp.Models;
using clinicautp.DTOs;
using clinicautp.Utilities;
using clinicautp.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace clinicautp.ViewModels
{
    public partial class CitaMainViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<CitaDTO> listaCitas = new ObservableCollection<CitaDTO>();

        public CitaMainViewModel(ClinicaDBContext context)
        {
            _dbContext = context;

            MainThread.BeginInvokeOnMainThread(new Action(async () => await ObtenerCitas()));
        }

        public async Task ObtenerCitas()
        {
            ListaCitas.Clear();

            var lista = await _dbContext.Citas
                .Where(c => c.CedulaPaciente == AppState.Instance.CedulaPaciente)
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

        [RelayCommand]
        private async Task CrearCita()
        {
            var uri = $"{nameof(PacienteCitaPage)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task EditarCita(CitaDTO citaDto)
        {
            var uri = $"{nameof(PacienteCitaPage)}?id={citaDto.Id}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task EliminarCita(CitaDTO citaDto)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "¿Eliminar Cita?", "Sí", "No");
            if (answer)
            {
                var encontrada = await _dbContext.Citas
                    .FirstOrDefaultAsync(c => c.Id == citaDto.Id);

                if (encontrada != null)
                {
                    _dbContext.Citas.Remove(encontrada);
                    await _dbContext.SaveChangesAsync();

                    ListaCitas.Remove(citaDto);
                }
            }
        }

        [RelayCommand]
        private async Task NavegarPerfil()
        {
            var cedula = AppState.Instance.CedulaPaciente;
            var uri = $"{nameof(ProfilePage)}?cedula={cedula}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task LogOut()
        {
            try
            {
                AppState.Instance.CedulaPaciente = null; // Limpiar la cédula del paciente
                await Shell.Current.GoToAsync("//LogInPage");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo cerrar sesión: {ex.Message}", "OK");
            }
        }
    }
}
