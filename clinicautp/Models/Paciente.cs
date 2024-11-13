using System.ComponentModel.DataAnnotations;

namespace clinicautp.Models
{
    public class Paciente
    {
        [Key]
        public string Cedula { get; set; }

        [Required]
        public string Contrasena { get; set; }

        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public string? Correo { get; set; }

        public string? Sangre { get; set; }

        public string? Telefono { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public ICollection<HistorialMedico> HistoriasMedicas { get; set; } = new List<HistorialMedico>();

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();

        public bool EsDonador { get; set;}
    }
}
