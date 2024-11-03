using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using clinicautp.DataAccess;
using clinicautp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace clinicautp.ViewModels
{
    public partial class MedicamentoRegisterViewModel : ObservableObject
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

        public MedicamentoRegisterViewModel(ClinicaDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [RelayCommand]
        private async Task AgregarMedicamento()
        {
            // Validar que todos los campos obligatorios estén completos
            if (string.IsNullOrWhiteSpace(codMedicamento) ||
                string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(dosis) ||
                fechaVencimiento == default ||
                cantidadDisponible <= 0 ||
                cantidadMinima < 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, complete todos los campos obligatorios.", "OK");
                return;
            }

            // Verificar si el medicamento ya existe por su código
            var existeMedicamento = await _dbContext.Medicamentos.AnyAsync(m => m.CodMedicamento == CodMedicamento);
            if (existeMedicamento)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El medicamento ya está registrado.", "OK");
                return;
            }

            // Crear un nuevo medicamento
            var nuevoMedicamento = new Medicamento
            {
                CodMedicamento = CodMedicamento,
                Nombre = Nombre,
                Dosis = Dosis,
                FechaVencimiento = FechaVencimiento,
                Indicaciones = Indicaciones,
                CantidadDisponible = CantidadDisponible,
                CantidadMinima = CantidadMinima
            };

            // Agregar el medicamento a la base de datos
            await _dbContext.Medicamentos.AddAsync(nuevoMedicamento);
            await _dbContext.SaveChangesAsync();

            // Mostrar mensaje de éxito
            await Application.Current.MainPage.DisplayAlert("Éxito", "Medicamento agregado correctamente.", "OK");
        }
    }
}
