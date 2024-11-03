using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace clinicautp.Models
{
    public class Referencias
    {
        [Key]
        public int IdReferencia { get; set; }
        [Required]
        public string CedulaPaciente { get; set; }
        public string NombrePaciente { get; set; }
        public string Especialidad { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha {  get; set; }
        [ForeignKey(nameof(CedulaPaciente))]
        public Paciente Paciente { get; set; }
    }
}
