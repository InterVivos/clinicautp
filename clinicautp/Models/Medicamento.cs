using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clinicautp.Models
{
    public class Medicamento
    {
        [Key]
        public string CodMedicamento { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string Dosis { get; set; }

        [Required]
        public DateTime FechaVencimiento { get; set; }

        public string? Indicaciones { get; set; }

        public int CantidadDisponible { get; set; }

        public int CantidadMinima { get; set; } // Cantidad mínima antes de emitir alerta de escasez

        // Propiedad para marcar si el medicamento está en estado crítico o escasez
        public bool EnEscasez => CantidadDisponible <= CantidadMinima;

        public List<HistorialMedico> HistorialesMedicos { get; } = [];
    }
}
