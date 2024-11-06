using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using clinicautp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class ReferenciaEspecialidadViewModel : ObservableObject
{
    public List<Referencias> Referencias { get; set; } = new List<Referencias>();

    public ReferenciaEspecialidadViewModel()
    {
        // Puedes agregar ejemplos de datos aquí
        Referencias.Add(new Referencias { Especialidad = "Cardiología", Descripcion = "Consulta general de corazón." });
        Referencias.Add(new Referencias { Especialidad = "Dermatología", Descripcion = "Tratamiento de enfermedades de la piel." });
    }

    [RelayCommand]
    public async Task GenerarPdf()
    {
        try
        {
            var pdfGenerator = new PdfGenerator();
            await pdfGenerator.CrearPDFReferenciaEspecialidadAsync(Referencias);

            //await Application.Current.MainPage.DisplayAlert("Éxito", "El PDF de referencias se ha generado exitosamente.", "OK");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo generar el PDF: {ex.ToString()}", "OK");
        }
    }
}
