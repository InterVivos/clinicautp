﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using clinicautp.DataAccess;
using clinicautp.DTOs;
using Microsoft.EntityFrameworkCore;
using clinicautp.Models;
using clinicautp.Utilities;
using System.Collections.ObjectModel;
using clinicautp.Views;

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

        [ObservableProperty]
        private ObservableCollection<MedicamentoAdministradoDTO> listaMedicamentos = new ObservableCollection<MedicamentoAdministradoDTO>();

        [ObservableProperty]
        private bool generarCertificado;

        [ObservableProperty]
        private bool vistaEsMedico = true;

        [ObservableProperty]
        private bool certificadoDisponible = false;

        private byte[] certificadoBuenaSalud { get; set;}

        public HistorialMedicoViewModel(ClinicaDBContext context)
        {
            _dbContext = context;
            Fecha = DateTime.Now;
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
                        certificadoBuenaSalud = encontrado.CertificadoBuenaSalud;
                        VistaEsMedico = false;
                        if(certificadoBuenaSalud != null && certificadoBuenaSalud.Length > 0) CertificadoDisponible = true;
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

                if (GenerarCertificado)
                {
                    var paciente = await _dbContext.Pacientes
                        .FirstOrDefaultAsync(p => p.Cedula == citaSel.CedulaPaciente);
                    var medico = await _dbContext.PersonalMedicos
                        .FirstOrDefaultAsync(pm => pm.Cedula == AppState.Instance.CedulaPersonalMedico);
                    certificadoBuenaSalud = await PdfGenerator.CrearPDFCertificadoBuenaSalud(citaSel.CedulaPaciente, $"{paciente.Nombre} {paciente.Apellido}", $"{medico.Nombre} {medico.Apellido}", Especialidad);
                }
                else certificadoBuenaSalud = [];

                if (idHistorialMedico == 0)
                {
                    // Crear un nuevo historial médico
                    var nuevoHistorialMedico = new HistorialMedico
                    {
                        //CedulaPaciente = AppState.Instance.CedulaPaciente, 
                        CedulaPaciente = citaSel.CedulaPaciente,
                        Fecha = Fecha,
                        Especialidad = Especialidad,
                        Detalles = Detalles,
                        CertificadoBuenaSalud = certificadoBuenaSalud
                    };

                    _dbContext.HistorialMedicos.Add(nuevoHistorialMedico);

                    foreach(var item in ListaMedicamentos)
                    {
                        if(item.Cantidad >= 1)
                        {
                            await _dbContext.SaveChangesAsync();
                            _dbContext.MedicamentosAdministrados.Add(new MedicamentoAdministrado
                            {
                                IdHistorial = nuevoHistorialMedico.IdHistorial,
                                CodMedicamento = item.Medicamento.CodMedicamento,
                                Cantidad = item.Cantidad
                            });
                            var medicamento = await _dbContext.Medicamentos.FirstOrDefaultAsync(e => e.CodMedicamento == item.Medicamento.CodMedicamento);
                            if (medicamento != null) medicamento.CantidadDisponible -= item.Cantidad;
                        }
                    }
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

        [RelayCommand]
        private async Task AdministrarMedicamentos()
        {
            if(!ListaMedicamentos.Any())
            {
                if(idHistorialMedico == 0)
                {
                    var lista = await _dbContext.Medicamentos
                        .Where(m => m.CantidadDisponible >= 1)
                        .ToListAsync();

                    if (lista.Count > 0)
                    {
                        foreach (var item in lista)
                        {
                            ListaMedicamentos.Add(new MedicamentoAdministradoDTO
                            {
                                IdHistorial = 0,
                                Medicamento = item
                            });
                        }
                    }
                }
                else
                {
                    var lista = await _dbContext.MedicamentosAdministrados
                        .Where(h => h.IdHistorial == idHistorialMedico)
                        .ToListAsync();
                    
                    if (lista.Count > 0)
                    {
                        foreach (var item in lista)
                        {
                            ListaMedicamentos.Add(new MedicamentoAdministradoDTO
                            {
                                IdHistorial = idHistorialMedico,
                                Medicamento = item.Medicamento,
                                Cantidad = item.Cantidad
                            });
                        }
                    }
                }
            }
            await Shell.Current.Navigation.PushAsync(new AdministrarMedicamentos(this));
        }

        [RelayCommand]
        private async Task DescargarCertificado()
        {
            await PdfGenerator.DescargarPdfBd(certificadoBuenaSalud, "CertificadoBuenaSalud");
        }
    }
}
