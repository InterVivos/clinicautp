using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace clinicautp.Models
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }  // Identificador único de la cita

        [Required]
        public string CedulaPaciente { get; set; }  // Cédula del paciente

        public DateTime FechaCita { get; set; }  // Fecha de la cita
        public TimeSpan HoraCita { get; set; }  // Hora de la cita

        public string Especialidad { get; set; }  // Especialidad de la consulta
        public string Estado { get; set; }  // Estado de la cita (e.g., Programada, Cancelada, Completada)
        public DateTime FechaCreacion { get; set; } = DateTime.Now;  // Fecha de creación de la cita
        public string Observaciones { get; set; }  // Observaciones adicionales sobre la cita

        // Relaciones
        [ForeignKey(nameof(CedulaPaciente))]
        public Paciente Paciente { get; set; }  // Relación con el modelo Paciente

        // public PersonalMedico PersonalMedico { get; set; }
    }
}
