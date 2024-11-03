using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using clinicautp.DataAccess;
using clinicautp.Models;
using clinicautp.DTOs;
using clinicautp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace clinicautp.ViewModels
{
    public partial class PacienteViewModel : ObservableObject, IQueryAttributable
    {

        private readonly ClinicaDBContext _dbContext;

        [ObservableProperty]
        private PacienteDTO pacienteDto = new PacienteDTO();
        
        [ObservableProperty]
        private string tituloPagina;

        private string Cedula;

        [ObservableProperty]
        private bool loadingIsVisible = false;

        public PacienteViewModel(ClinicaDBContext context)
        {
            _dbContext = context;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var cedula = query["cedula"].ToString();
            Cedula = cedula;

            if (string.IsNullOrEmpty(Cedula))
            {
                TituloPagina = "Nuevo Paciente";
            }
            else
            {
                TituloPagina = "Actualizar Paciente";
                LoadingIsVisible = true;

                try
                {
                    var paciente = await _dbContext.Pacientes.FirstOrDefaultAsync(e => e.Cedula == Cedula);
                    if (paciente != null)
                    {
                        PacienteDto = new PacienteDTO
                        {
                            cedula = paciente.Cedula,
                            nombre = paciente.Nombre,
                            apellido = paciente.Apellido,
                            sangre = paciente.Sangre,
                            contrasena = paciente.Contrasena,
                            correo = paciente.Correo,
                            telefono = paciente.Telefono,
                            fechaNacimiento = paciente.FechaNacimiento
                        };
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine($"Error al cargar paciente: {ex.Message}");
                }
                finally
                {
                    MainThread.BeginInvokeOnMainThread(() => { LoadingIsVisible = false; });
                }
            }
        }

        [RelayCommand]
        private async Task Guardar()
        {
            LoadingIsVisible = true;

            await Task.Run(async () =>
            {

                var pacienteExistente = await _dbContext.Pacientes.FirstOrDefaultAsync(p => p.Cedula == PacienteDto.cedula);

                if (pacienteExistente == null)
                {
                    var nuevoPaciente = new Paciente
                    {
                        Cedula = PacienteDto.cedula,
                        Nombre = PacienteDto.nombre,
                        Apellido = PacienteDto.apellido,
                        Contrasena = PacienteDto.contrasena,
                        Sangre = PacienteDto.sangre,
                        Telefono = PacienteDto.telefono,
                        Correo = PacienteDto.correo,
                        FechaNacimiento = PacienteDto.fechaNacimiento
                    };

                    _dbContext.Pacientes.Add(nuevoPaciente);
                    await _dbContext.SaveChangesAsync();

                    //mensaje = new PacienteMessage()
                    //{
                    //    IsCreated = true,
                    //    PacienteDto = PacienteDto
                    //};
                }
                else
                {
                    // Si ya existe, actualiza la información del paciente
                    pacienteExistente.Nombre = PacienteDto.nombre;
                    pacienteExistente.Apellido = PacienteDto.apellido;
                    pacienteExistente.Correo = PacienteDto.correo;
                    pacienteExistente.Contrasena = PacienteDto.contrasena;
                    pacienteExistente.Telefono = PacienteDto.telefono;
                    pacienteExistente.Sangre = PacienteDto.sangre;
                    pacienteExistente.FechaNacimiento = PacienteDto.fechaNacimiento;
                    await _dbContext.SaveChangesAsync();

                    //mensaje = new PacienteMessage()
                    //{
                    //    IsCreated = false,
                    //    PacienteDto = PacienteDto
                    //};
                }

                // Actualiza la visibilidad del indicador de carga y envía el mensaje de actualización
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    LoadingIsVisible = false;
                    //WeakReferenceMessenger.Default.Send(new PacienteMessaging(mensaje));
                    await Shell.Current.Navigation.PopAsync();
                });
            });
        }

    }
}
