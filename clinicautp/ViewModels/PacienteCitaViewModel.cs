using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using clinicautp.DataAccess;
using clinicautp.Models;
using clinicautp.Utilities;

namespace clinicautp.ViewModels
{
    public partial class PacienteCitaViewModel : ObservableObject, IQueryAttributable
    {
        private readonly ClinicaDBContext _dbContext;

        [ObservableProperty]
        private DateTime fechaCita;  // Fecha de la cita

        [ObservableProperty]
        private TimeSpan horaCita;  // Hora de la cita

        [ObservableProperty]
        private ObservableCollection<string> especialidades;  // Opciones de especialidad

        [ObservableProperty]
        private string especialidadSeleccionada;  // Especialidad seleccionada

        [ObservableProperty]
        private string estado;  // Estado de la cita

        [ObservableProperty]
        private string observaciones;  // Observaciones adicionales sobre la cita

        private int idCita;

        public PacienteCitaViewModel(ClinicaDBContext context)
        {
            _dbContext = context;

            FechaCita = DateTime.Now;

            HoraCita = DateTime.Now.TimeOfDay;

            // Lista de especialidades para seleccionar
            Especialidades = new ObservableCollection<string>
            {
                "Consulta General",
                "Urgencias",
                "Ginecología",
                "Dermatología",
                "Neurología",
                "Oftalmología",
                "Ortopedia"
            };
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("id") && int.TryParse(query["id"].ToString(), out int parsedId))
            {
                idCita = parsedId;

                if (idCita != 0)
                {
                    var citaEncontrada = await _dbContext.Citas
                        .FirstOrDefaultAsync(c => c.Id == idCita);

                    if (citaEncontrada != null)
                    {
                        // Asignar los valores de la cita a las propiedades
                        FechaCita = citaEncontrada.FechaCita;  // Asignar la fecha
                        HoraCita = citaEncontrada.HoraCita;  // Asignar la hora
                        EspecialidadSeleccionada = citaEncontrada.Especialidad;
                        Estado = citaEncontrada.Estado;
                        Observaciones = citaEncontrada.Observaciones;
                    }
                }
            }
        }

        [RelayCommand]
        private async Task Guardar()
        {
            try
            {
                if (idCita == 0)
                {
                    // Crear una nueva cita
                    var nuevaCita = new Cita
                    {
                        CedulaPaciente = AppState.Instance.CedulaPaciente,  // Asignar la cédula del paciente
                        FechaCita = FechaCita,  // Asignar la fecha
                        HoraCita = HoraCita,  // Asignar la hora
                        Especialidad = EspecialidadSeleccionada ?? throw new ArgumentNullException(nameof(especialidadSeleccionada), "La especialidad es obligatoria."),  // Guardar la especialidad seleccionada
                        Estado = Estado ?? "Programada",  // Estado predeterminado como "Programada"
                        Observaciones = Observaciones,
                        FechaCreacion = DateTime.Now  // Establecer la fecha de creación
                    };

                    _dbContext.Citas.Add(nuevaCita);
                }
                else
                {
                    // Actualizar la cita existente
                    var citaEncontrada = await _dbContext.Citas
                        .FirstOrDefaultAsync(c => c.Id == idCita);

                    if (citaEncontrada != null)
                    {
                        citaEncontrada.FechaCita = FechaCita;  // Actualizar fecha
                        citaEncontrada.HoraCita = HoraCita;  // Actualizar hora
                        citaEncontrada.Especialidad = EspecialidadSeleccionada ?? throw new ArgumentNullException(nameof(especialidadSeleccionada), "La especialidad es obligatoria.");  // Actualizar especialidad seleccionada
                        citaEncontrada.Estado = Estado;
                        citaEncontrada.Observaciones = Observaciones;
                        // No se actualiza FechaCreacion porque no debe cambiar
                    }
                }

                await _dbContext.SaveChangesAsync();

                // Navegar hacia atrás después de guardar
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
