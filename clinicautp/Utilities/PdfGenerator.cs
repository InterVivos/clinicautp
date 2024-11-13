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
using System.Runtime.CompilerServices;
using System.Globalization;


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
            await Toast.Make($"No se guardó el PDF: {fileSaverResult.Exception.Message}").Show(cancellationToken);
        }
    }

    public static async Task<byte[]> CrearPDFCertificadoBuenaSalud(string cedulaPaciente, string nombrePaciente, string nombreMedico, string especialidad)
    {   
        var document = new PdfDocument();
        var page = document.AddPage();
        page.Size = PageSize.A4;
        page.Orientation = PageOrientation.Portrait;

        var graphics = XGraphics.FromPdfPage(page);
        var font = new XFont("OpenSans", 20, XFontStyle.Regular);

        graphics.DrawString("Certificado de Buena Salud", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.TopCenter);

        font = new XFont("OpenSans", 12, XFontStyle.Regular);
        int yPoint = 40;

        graphics.DrawString($"El suscrito médico certifica que {nombrePaciente}, portador (a) del documento de identidad nro. {cedulaPaciente},", font, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        yPoint += 20;
        graphics.DrawString($"se encuentra en buen estado de salud y no es portador (a) de enfermedades infectocontagiosas.", font, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        yPoint += 20;   
        DateTime fechaActual = DateTime.Today;
        graphics.DrawString($"Se expide el certificado en la Ciudad de David a los {fechaActual.Day} días del mes de {fechaActual.ToString("MMMM", CultureInfo.InvariantCulture)} de {fechaActual.Year}.", font, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        yPoint += 40;
        graphics.DrawString($"Dr. {nombreMedico}", font, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        yPoint += 20;
        graphics.DrawString($"{especialidad}", font, XBrushes.Black, new XRect(0, yPoint, page.Width, page.Height), XStringFormats.TopLeft);

        //var folderPath = FileSystem.AppDataDirectory; // Usar el directorio de datos de la aplicación
        //var filePath = Path.Combine(folderPath, "ReferenciasEspecialidades.pdf");
        //document.Save(filePath);

        using var stream = new MemoryStream();
        document.Save(stream, false);

        byte[] bytes;
        using (var binaryReader = new BinaryReader(stream))
        {
            bytes = binaryReader.ReadBytes((int)stream.Length);
        }
        return bytes;
    }

    public static async Task DescargarPdfBd(byte[] pdf, string nombreArchivo)
    {
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

        using var stream = new MemoryStream(pdf);
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken cancellationToken = source.Token;

        var fileSaverResult = await FileSaver.Default.SaveAsync($"{nombreArchivo}.pdf", stream, cancellationToken);
        if (fileSaverResult.IsSuccessful)
        {
            await Toast.Make($"Archivo PDF guardado en: {fileSaverResult.FilePath}").Show(cancellationToken);
        }
        else
        {
            await Toast.Make($"No se guardó el PDF: {fileSaverResult.Exception.Message}").Show(cancellationToken);
        }
    }
}
