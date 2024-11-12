using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using clinicautp.DataAccess;
using clinicautp.DTOs;
using Microsoft.EntityFrameworkCore;
using clinicautp.Models;
using clinicautp.Utilities;

namespace clinicautp.ViewModels
{
    public partial class HistorialMedicoViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ClinicaDBContext _dbContext;

        [ObservableProperty]
        private DateTime fecha;

        [ObservableProperty]
        private string especialidad;

        [ObservableProperty]
        private string detalles;

        public int idHistorialMedico {get; private set;}

        public HistorialMedicoViewModel(ClinicaDBContext context)
        {
            _dbContext = context;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("id") && int.TryParse(query["id"].ToString(), out int parsedId))
            {
                idHistorialMedico = parsedId;

                if (idHistorialMedico != 0)
                {
                    var encontrado = await _dbContext.HistorialMedicos
                        .FirstOrDefaultAsync(e => e.IdHistorial == idHistorialMedico);

                    if (encontrado != null)
                    {

                        Fecha = encontrado.Fecha;
                        Especialidad = encontrado.Especialidad;
                        Detalles = encontrado.Detalles;
                    }
                }
            }
        }

        [RelayCommand]
        private async Task Guardar()
        {
            try
            {
                var citaSel = await _dbContext.Citas
                        .FirstOrDefaultAsync(c => c.Id == AppState.Instance.IdCitaSeleccionada);

                if (idHistorialMedico == 0)
                {
                    // Crear un nuevo historial médico
                    var nuevoHistorialMedico = new HistorialMedico
                    {
                        //CedulaPaciente = AppState.Instance.CedulaPaciente, 
                        CedulaPaciente = citaSel.CedulaPaciente,
                        Fecha = fecha,
                        Especialidad = especialidad,
                        Detalles = detalles
                    };

                    _dbContext.HistorialMedicos.Add(nuevoHistorialMedico);
                }
                else
                {
                    var encontrado = await _dbContext.HistorialMedicos
                        .FirstOrDefaultAsync(e => e.IdHistorial == idHistorialMedico);

                    if (encontrado != null)
                    {
                        encontrado.Fecha = Fecha;
                        encontrado.Especialidad = Especialidad;
                        encontrado.Detalles = Detalles;
                    }
                }

                citaSel.Estado = "Completada";
                AppState.Instance.IdCitaSeleccionada = 0;

                await _dbContext.SaveChangesAsync();

                MessagingCenter.Send(this, "CitaCompletada", citaSel);

                await Shell.Current.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "No InnerException";
                await Shell.Current.DisplayAlert("Error", $"Ocurrió un error al guardar: {ex.Message}\nInnerException: {innerExceptionMessage}", "OK");
            }
        }

    }
}
