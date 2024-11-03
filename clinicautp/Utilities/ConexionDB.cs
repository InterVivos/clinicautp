using System;
using System.IO;
using Microsoft.Maui.Devices;  // Referencia para DeviceInfo

namespace clinicautp.Utilities
{
    public static class ConexionDB
    {
        public static string ReturnRoute(string nombreBD)
        {
            // Variable para almacenar la ruta de la base de datos
            string routeBD = string.Empty;

            try
            {
                // Verificar si la plataforma es Android
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    // Obtener la ruta para datos de aplicación local en Android
                    routeBD = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    // Combinar la ruta con el nombre de la base de datos
                    routeBD = Path.Combine(routeBD, nombreBD);
                }
                // Verificar si la plataforma es iOS
                else if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    // Obtener la ruta para documentos en iOS
                    routeBD = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    // Combinar la ruta con el nombre de la base de datos y la ruta "Library" en iOS
                    routeBD = Path.Combine(routeBD, "..", "Library", nombreBD);
                }
                // Opcionalmente, manejar otras plataformas como Windows o MacOS
                else if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    // Obtener la ruta de documentos en Windows
                    routeBD = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    routeBD = Path.Combine(routeBD, nombreBD);
                }
                else if (DeviceInfo.Platform == DevicePlatform.MacCatalyst)
                {
                    // Obtener la ruta de documentos en MacCatalyst
                    routeBD = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    routeBD = Path.Combine(routeBD, nombreBD);
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, puedes registrar el error o manejarlo de otra manera
                Console.WriteLine($"Error obteniendo la ruta de la base de datos: {ex.Message}");
            }

            // Devolver la ruta completa de la base de datos
            return routeBD;
        }
    }
}
