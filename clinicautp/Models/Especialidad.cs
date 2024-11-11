using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using clinicautp.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace clinicautp.Models
{
    public class Especialidad
    {
        [Key]
        public required string Nombre { get; set; }

        public ICollection<PersonalMedico> cedulaMedico { get; } = new List<PersonalMedico>();

        [SetsRequiredMembers]
        public Especialidad(string nombre)
        {
            Nombre = nombre;
        }

        public static async Task LoadEspecialidades(ClinicaDBContext _dbContext, ObservableCollection<string> listaEspecialidades)
        {
            
            listaEspecialidades.Clear();

            
            var lista = await _dbContext.Especialidades
                .ToListAsync();

            if (lista.Any())
            {
                foreach (var especialidad in lista)
                {
                    listaEspecialidades.Add(especialidad.Nombre);
                }
            }
        }
    }
}