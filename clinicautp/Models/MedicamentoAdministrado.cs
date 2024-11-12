using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clinicautp.Models
{
    public class MedicamentoAdministrado
    {
        [Key]
        public int Id { get; set;}

        [Required]
        public int IdHistorial { get; set; }

        [ForeignKey(nameof(IdHistorial))]
        public HistorialMedico HistorialMedico { get; set; }

        [Required]
        public string CodMedicamento { get; set; }

        [ForeignKey(nameof(CodMedicamento))]
        public Medicamento Medicamento { get; set; }

        public int Cantidad { get; set; }
    }
}
