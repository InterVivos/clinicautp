namespace clinicautp.DTOs
{
    public class MedicamentoDTO
    {
        public string codMedicamento { get; set; }
        public string nombre { get; set; }
        public string dosis { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public string? indicaciones { get; set; }
        public int cantidadDisponible { get; set; }
        public int cantidadMinima { get; set; }
        public bool enEscasez { get; set; }
    }
}
