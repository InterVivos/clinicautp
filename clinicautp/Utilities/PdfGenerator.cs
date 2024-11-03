using clinicautp.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using PdfSharp; // Asegúrate de importar este espacio de nombres para FileSystem.AppDataDirectory

public class PdfGenerator
{
    public async Task CrearPDFReferenciaEspecialidadAsync(List<Referencias> referencias)
    {
        if (referencias == null || referencias.Count == 0)
        {
            throw new InvalidOperationException("No hay referencias disponibles para generar el PDF.");
        }

        var document = new PdfDocument();
        var page = document.AddPage();
        page.Size = PageSize.A4;
        page.Orientation = PageOrientation.Portrait;

        var graphics = XGraphics.FromPdfPage(page);
        var font = new XFont("Arial", 20);

        graphics.DrawString("Referencias a Especialidades Médicas", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.TopCenter);

        font = new XFont("Arial", 12);
        int yPoint = 40;

        foreach (var referencia in referencias)
        {
            if (referencia == null)
            {
                continue; // O lanza una excepción si prefieres
            }

            graphics.DrawString($"Especialidad: {referencia.Especialidad}", font, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            graphics.DrawString($"Descripcion: {referencia.Descripcion}", font, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 40; // Espacio entre referencias
        }

        var folderPath = FileSystem.AppDataDirectory; // Usar el directorio de datos de la aplicación
        var filePath = Path.Combine(folderPath, "ReferenciasEspecialidades.pdf");
        document.Save(filePath);
    }
}
