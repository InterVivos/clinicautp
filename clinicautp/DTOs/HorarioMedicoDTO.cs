
namespace clinicautp.DTOs
{
    public partial class HorarioMedicoDTO
    {
        public string idHorario { get; set; }
        public string cedulaMedico { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin {  get; set; }

    }
}
