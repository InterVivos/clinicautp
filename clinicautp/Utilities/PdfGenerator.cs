using clinicautp.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using CommunityToolkit.Maui;
using PdfSharpCore;
using PdfSharpCore.Fonts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Alerts;
#if ANDROID
using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
#endif
 // Asegúrate de importar este espacio de nombres para FileSystem.AppDataDirectory

public class PdfGenerator
{
    
    public async Task CrearPDFReferenciaEspecialidadAsync(List<Referencias> referencias)
    {
        if (referencias == null || referencias.Count == 0)
        {
            throw new InvalidOperationException("No hay referencias disponibles para generar el PDF.");
        }

        #if ANDROID
        if (DeviceInfo.Platform == DevicePlatform.Android && OperatingSystem.IsAndroidVersionAtLeast(33))
        {
            var activity = Platform.CurrentActivity ?? throw new NullReferenceException("Current activity is null");
            if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.ReadExternalStorage }, 1);
            }
        }
        #endif
        
        var document = new PdfDocument();
        var page = document.AddPage();
        page.Size = PageSize.A4;
        page.Orientation = PageOrientation.Portrait;

        var graphics = XGraphics.FromPdfPage(page);
        var font = new XFont("OpenSans", 20, XFontStyle.Regular);

        graphics.DrawString("Referencias a Especialidades Médicas", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.TopCenter);

        font = new XFont("OpenSans", 12, XFontStyle.Regular);
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

        //var folderPath = FileSystem.AppDataDirectory; // Usar el directorio de datos de la aplicación
        //var filePath = Path.Combine(folderPath, "ReferenciasEspecialidades.pdf");
        //document.Save(filePath);

        using var stream = new MemoryStream();
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken cancellationToken = source.Token;
        document.Save(stream, false);

        var fileSaverResult = await FileSaver.Default.SaveAsync("ReferenciasEspecialidades.pdf", stream, cancellationToken);
        if (fileSaverResult.IsSuccessful)
        {
            await Toast.Make($"Archivo PDF guardado en: {fileSaverResult.FilePath}").Show(cancellationToken);
        }
        else
        {
            await Toast.Make($"No se pudo guardar el PDF: {fileSaverResult.Exception.Message}").Show(cancellationToken);
        }
    }
}
