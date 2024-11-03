namespace clinicautp.DTOs
{
    public class CitaDTO
    {
        public int Id { get; set; }  // Identificador único de la cita
        public string CedulaPaciente { get; set; }  // Cédula del paciente
        public DateTime FechaCita { get; set; }  // Fecha de la cita
        public TimeSpan HoraCita { get; set; }  // Hora de la cita
        public string Especialidad { get; set; }  // Especialidad de la consulta
        public string Estado { get; set; }  // Estado de la cita (e.g., Programada, Cancelada, Completada)
        public DateTime FechaCreacion { get; set; }  // Fecha de creación de la cita
        public string Observaciones { get; set; }  // Observaciones adicionales sobre la cita
    }
}
