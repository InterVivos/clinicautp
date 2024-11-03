using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clinicautp.Models
{
    public class HistorialMedico
    {
        [Key]
        public int IdHistorial { get; set; }

        [Required]
        public string CedulaPaciente { get; set; }

        public DateTime Fecha { get; set; }

        public string? Especialidad { get; set; }

        [Required]
        public string Detalles { get; set; }

        [ForeignKey(nameof(CedulaPaciente))]
        public Paciente Paciente { get; set; }
    }
}
