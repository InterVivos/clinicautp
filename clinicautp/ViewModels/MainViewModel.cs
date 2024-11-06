using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;

using clinicautp.DataAccess;
using clinicautp.Models;
using clinicautp.Utilities;
using clinicautp.Views;
using clinicautp.DTOs;
using System.Collections.ObjectModel;

namespace clinicautp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<HistorialMedicoDTO> listaHistorialMedico = new ObservableCollection<HistorialMedicoDTO>();

        public MainViewModel(ClinicaDBContext context)
        {
            _dbContext = context;

            MainThread.BeginInvokeOnMainThread(new Action(async () => await ObtenerHistorialMedico()));
        }

        public async Task ObtenerHistorialMedico()
        {
            // Limpiar la lista antes de agregar nuevos datos
            ListaHistorialMedico.Clear();

            // Obtener solo los historiales médicos relacionados con el paciente logueado
            var lista = await _dbContext.HistorialMedicos
                .Where(h => h.CedulaPaciente == AppState.Instance.CedulaPaciente)
                .ToListAsync();

            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    ListaHistorialMedico.Add(new HistorialMedicoDTO
                    {
                        idHistorial = item.IdHistorial,
                        fecha = item.Fecha,
                        especialidad = item.Especialidad,
                        detalles = item.Detalles,
                        cedulaPaciente = item.CedulaPaciente
                    });
                }
            }
        }

        // Comando para crear un historial médico
        [RelayCommand]
        private async Task CrearHistorial()
        {
            var uri = $"{nameof(HistorialMedicoPage)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task EditarHistorial(HistorialMedicoDTO historialDto)
        {
            var uri = $"{nameof(HistorialMedicoPage)}?id={historialDto.idHistorial}";
            await Shell.Current.GoToAsync(uri);
        }


        [RelayCommand]
        private async Task EliminarHistorial(HistorialMedicoDTO historialDto)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "¿Eliminar Historial Médico?", "Sí", "No");
            if (answer)
            {
                var encontrado = await _dbContext.HistorialMedicos
                    .FirstOrDefaultAsync(e => e.IdHistorial == historialDto.idHistorial);

                if (encontrado != null)
                {
                    _dbContext.HistorialMedicos.Remove(encontrado);
                    await _dbContext.SaveChangesAsync();

                    ListaHistorialMedico.Remove(historialDto);
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

        // Comando para cerrar sesión
        [RelayCommand]
        private async Task LogOut()
        {
            try
            {
                //await Shell.Current.GoToAsync("LogInPage");
                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo cerrar sesión: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task IrCita()
        {
            var uri = $"{nameof(CitaMainViewPage)}";
            await Shell.Current.GoToAsync(uri);
        }
    }
}