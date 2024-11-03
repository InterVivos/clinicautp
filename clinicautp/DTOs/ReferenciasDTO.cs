using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinicautp.DTOs
{
    public partial class ReferenciasDTO
    {
        public int IdReferencia { get; set; }
        public string CedulaPaciente { get; set; }
        public string NombrePaciente { get; set; }
        public string Especialidad { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
