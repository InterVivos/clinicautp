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

            fechaCita = DateTime.Now;

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
                        fechaCita = citaEncontrada.FechaCita;  // Asignar la fecha
                        horaCita = citaEncontrada.HoraCita;  // Asignar la hora
                        especialidadSeleccionada = citaEncontrada.Especialidad;
                        estado = citaEncontrada.Estado;
                        observaciones = citaEncontrada.Observaciones;
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
                        FechaCita = fechaCita,  // Asignar la fecha
                        HoraCita = horaCita,  // Asignar la hora
                        Especialidad = especialidadSeleccionada ?? throw new ArgumentNullException(nameof(especialidadSeleccionada), "La especialidad es obligatoria."),  // Guardar la especialidad seleccionada
                        Estado = estado ?? "Programada",  // Estado predeterminado como "Programada"
                        Observaciones = observaciones,
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
                        citaEncontrada.FechaCita = fechaCita;  // Actualizar fecha
                        citaEncontrada.HoraCita = horaCita;  // Actualizar hora
                        citaEncontrada.Especialidad = especialidadSeleccionada ?? throw new ArgumentNullException(nameof(especialidadSeleccionada), "La especialidad es obligatoria.");  // Actualizar especialidad seleccionada
                        citaEncontrada.Estado = estado;
                        citaEncontrada.Observaciones = observaciones;
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
