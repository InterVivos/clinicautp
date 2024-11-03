using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using clinicautp.DataAccess;
using clinicautp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace clinicautp.ViewModels
{
    public partial class MedicamentoActualizarViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

        // Propiedades para los datos del medicamento
        [ObservableProperty] private string codMedicamento;
        [ObservableProperty] private string nombre;
        [ObservableProperty] private string dosis;
        [ObservableProperty] private DateTime fechaVencimiento;
        [ObservableProperty] private string indicaciones;
        [ObservableProperty] private int cantidadDisponible;
        [ObservableProperty] private int cantidadMinima;

        // Constructor con la base de datos
        public MedicamentoActualizarViewModel(ClinicaDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // Método para cargar los datos del medicamento
        public async Task LoadMedicamentoAsync(string codigo)
        {
            try
            {
                // Buscar el medicamento por su código
                var medicamento = await _dbContext.Medicamentos
                    .FirstOrDefaultAsync(m => m.CodMedicamento == codigo);

                if (medicamento != null)
                {
                    // Asignar valores a las propiedades del ViewModel
                    CodMedicamento = medicamento.CodMedicamento;
                    Nombre = medicamento.Nombre;
                    Dosis = medicamento.Dosis;
                    FechaVencimiento = medicamento.FechaVencimiento;
                    Indicaciones = medicamento.Indicaciones;
                    CantidadDisponible = medicamento.CantidadDisponible;
                    CantidadMinima = medicamento.CantidadMinima;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Medicamento no encontrado.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al cargar el medicamento: {ex.Message}", "OK");
            }
        }

        // Comando para actualizar los datos del medicamento
        [RelayCommand]
        private async Task ActualizarMedicamento()
        {
            // Validar que los campos estén completos
            if (string.IsNullOrWhiteSpace(CodMedicamento) || string.IsNullOrWhiteSpace(Nombre) ||
                string.IsNullOrWhiteSpace(Dosis) || FechaVencimiento == default ||
                CantidadDisponible <= 0 || CantidadMinima < 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, complete todos los campos obligatorios.", "OK");
                return;
            }

            try
            {
                // Buscar el medicamento por su código
                var medicamento = await _dbContext.Medicamentos
                    .FirstOrDefaultAsync(m => m.CodMedicamento == CodMedicamento);

                if (medicamento != null)
                {
                    // Actualizar las propiedades del medicamento
                    medicamento.Nombre = Nombre;
                    medicamento.Dosis = Dosis;
                    medicamento.FechaVencimiento = FechaVencimiento;
                    medicamento.Indicaciones = Indicaciones;
                    medicamento.CantidadDisponible = CantidadDisponible;
                    medicamento.CantidadMinima = CantidadMinima;

                    // Guardar los cambios en la base de datos
                    _dbContext.Medicamentos.Update(medicamento);
                    await _dbContext.SaveChangesAsync();

                    // Mostrar mensaje de éxito
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Medicamento actualizado correctamente.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Medicamento no encontrado.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al actualizar el medicamento: {ex.Message}", "OK");
            }
        }
    }
}
