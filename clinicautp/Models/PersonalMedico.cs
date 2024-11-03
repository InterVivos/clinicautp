using System.ComponentModel.DataAnnotations;

namespace clinicautp.Models
{
    public class PersonalMedico
    {
        [Key]
        public string Cedula { get; set; }

        [Required]
        public string Contrasena { get; set; }

        public string Cargo { get; set; }

        public string? Especialidad { get; set; }

        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public string? Correo { get; set; }

        public string? Telefono { get; set; }
    }
}
