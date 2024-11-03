using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clinicautp.Models
{
    public class HorarioMedico
    {
        [Key]
        public string IdHorario { get; set; }

        [Required]
        public string CedulaMedico { get; set; }
        public DateTime Fecha {  get; set; }
        [Required]
        public TimeSpan HoraInicio { get; set; }
        [Required]
        public TimeSpan HoraFin {  get; set; }
        [ForeignKey(nameof(CedulaMedico))]
        public PersonalMedico PersonalMedico { get; set; }
    }
}
