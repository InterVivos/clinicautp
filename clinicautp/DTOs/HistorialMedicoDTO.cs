namespace clinicautp.DTOs
{
    public partial class HistorialMedicoDTO
    {
        public int idHistorial { get; set; }

        public string cedulaPaciente { get; set; }

        public DateTime fecha { get; set; }

        public string? especialidad { get; set; }

        public string detalles { get; set; }
    }
}
