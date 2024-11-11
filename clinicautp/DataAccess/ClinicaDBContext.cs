using clinicautp.DTOs;
using clinicautp.Models;
using clinicautp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace clinicautp.DataAccess
{
    public class ClinicaDBContext : DbContext
    {
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<HistorialMedico> HistorialMedicos { get; set; }
        public DbSet<PersonalMedico> PersonalMedicos { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }

        public DbSet<HorarioMedico> HorarioMedico { get; set; }

        public DbSet<Referencias> Referencias { get; set; }

        public DbSet<Especialidad> Especialidades { get; set; }

        // Método que configura las opciones de la base de datos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Construcción de la cadena de conexión a la base de datos SQLite utilizando una ruta devuelta por la utilidad ConexionDB
            string conexionDB = $"Filename={ConexionDB.ReturnRoute("Clinica399.db")}";

            // Configura el contexto para usar SQLite con la cadena de conexión proporcionada
            optionsBuilder.UseSqlite(conexionDB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura la clave primaria para Paciente
            modelBuilder.Entity<Paciente>()
                .HasKey(p => p.Cedula); // Configura Cedula como la clave primaria

            // Configura la relación uno a muchos entre Paciente e HistorialMedico
            modelBuilder.Entity<Paciente>()
                .HasMany(p => p.HistoriasMedicas)
                .WithOne(h => h.Paciente)
                .HasForeignKey(h => h.CedulaPaciente);

            // Configura la cédula en PersonalMedico como única
            modelBuilder.Entity<PersonalMedico>()
                .HasIndex(p => p.Cedula)
                .IsUnique(); // Asegúrate de que la cédula sea única

            // Configura la relación uno a muchos entre Paciente y Cita
            modelBuilder.Entity<Paciente>()
                .HasMany(p => p.Citas)
                .WithOne(c => c.Paciente)
                .HasForeignKey(c => c.CedulaPaciente);

            // Configura las propiedades de Cita
            modelBuilder.Entity<Cita>()
                .HasKey(c => c.Id); // Configura Id como la clave primaria

            // Configuración de Medicamento
            modelBuilder.Entity<Medicamento>()
                .HasKey(m => m.CodMedicamento); // Configura CodMedicamento como la clave primaria
            
            modelBuilder.Entity<Especialidad>()
                .HasMany(e => e.cedulaMedico)
                .WithOne(e => e.Especialidad)
                .HasForeignKey(e => e.EspecialidadNombre)
                .IsRequired();
            
            modelBuilder.Entity<Especialidad>().HasData(
                new Especialidad("Consulta General"),
                new Especialidad("Urgencias"),
                new Especialidad("Ginecología"),
                new Especialidad("Dermatología"),
                new Especialidad("Neurología"),
                new Especialidad("Oftalmología"),
                new Especialidad("Ortopedia")
            );

            modelBuilder.Entity<PersonalMedico>().HasData(
                new PersonalMedico{Cedula="m", Nombre="m", Apellido="m", Cargo="m", Contrasena="m", Correo="m", EspecialidadNombre="Ortopedia", Telefono="m"}
            );

            modelBuilder.Entity<Paciente>().HasData(
                new Paciente{Cedula="e", Nombre="m", Apellido="m", Contrasena="e", Correo="m", FechaNacimiento=DateTime.Now, Sangre="B+"}
            );

            modelBuilder.Entity<Cita>().HasData(
                new Cita{CedulaPaciente="e", Especialidad="Ortopedia", Estado="Prog", FechaCita= DateTime.Now, HoraCita=TimeSpan.Zero, FechaCreacion=DateTime.Now, Observaciones="m", Id=1},
                new Cita{CedulaPaciente="e", Especialidad="Ortopedia", Estado="Prog", FechaCita= DateTime.Now, HoraCita=TimeSpan.Zero, FechaCreacion=DateTime.Now, Observaciones="m", Id=2},
                new Cita{CedulaPaciente="e", Especialidad="Ortopedia", Estado="Prog", FechaCita= DateTime.Now, HoraCita=TimeSpan.Zero, FechaCreacion=DateTime.Now, Observaciones="m", Id=3},
                new Cita{CedulaPaciente="e", Especialidad="Ortopedia", Estado="Prog", FechaCita= DateTime.Now, HoraCita=TimeSpan.Zero, FechaCreacion=DateTime.Now, Observaciones="m", Id=4},
                new Cita{CedulaPaciente="e", Especialidad="Ginecología", Estado="Prog", FechaCita= DateTime.Now, HoraCita=TimeSpan.Zero, FechaCreacion=DateTime.Now, Observaciones="m", Id=5}
            );
        }
    }
}
