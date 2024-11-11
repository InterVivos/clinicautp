using System;

namespace clinicautp.Utilities
{
    public class AppState
    {
        // Implementa el patrón Singleton para asegurarte de que solo haya una instancia de esta clase
        public static AppState Instance { get; } = new AppState();

        // Propiedad para almacenar la cédula del paciente
        public string CedulaPaciente { get; set; }

        // Propiedad para almacenar la cédula del personal médico
        public string CedulaPersonalMedico { get; set; }

        // Propiedad para almacenar el código del medicamento
        public string CodMedicamento { get; set; }

        // Propiedades predefinidas para el usuario y contraseña de admin
        public string AdminUsuario { get; set; } = "admin";
        public string AdminContrasena { get; set; } = "admin";

        // Método para validar si es administrador
        public bool EsAdmin(string usuario, string contrasena)
        {
            return usuario == AdminUsuario && contrasena == AdminContrasena;
        }

        public int IdCitaSeleccionada { get; set; }
    }
}
