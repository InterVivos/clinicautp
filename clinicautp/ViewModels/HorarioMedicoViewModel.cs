using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using clinicautp.DataAccess;
using clinicautp.DTOs;
using Microsoft.EntityFrameworkCore;
using clinicautp.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class HorarioMedicoViewModel : ObservableObject
{
    private readonly ClinicaDBContext _context;

    public ObservableCollection<HorarioMedico> Horarios { get; } = new();
    public ObservableCollection<PersonalMedico> PersonalMedicoList { get; } = new();

    // Propiedades para los controles de la vista
    [ObservableProperty]
    private DateTime _fecha;

    [ObservableProperty]
    private TimeSpan _horaInicio;

    [ObservableProperty]
    private TimeSpan _horaFin;

    [ObservableProperty]
    private PersonalMedico _medicoSeleccionado;

    public HorarioMedicoViewModel(ClinicaDBContext context)
    {
        _context = context;
        _ = CargarPersonalMedicoAsync(); // Carga personal médico de manera asincrónica
        _ = CargarHorariosAsync(); // Carga horarios de manera asincrónica
    }

    // Cargar la lista de médicos desde la base de datos de manera asincrónica
    public async Task CargarPersonalMedicoAsync()
    {
        var personal = await _context.PersonalMedicos.ToListAsync();
        PersonalMedicoList.Clear();
        foreach (var medico in personal)
        {
            PersonalMedicoList.Add(medico);
        }
    }

    // Cargar los horarios existentes de manera asincrónica
    public async Task CargarHorariosAsync()
    {
        var horarios = await _context.HorarioMedico.ToListAsync();
        Horarios.Clear();
        foreach (var horario in horarios)
        {
            Horarios.Add(horario);
        }
    }

    // Comando para asignar un horario a un médico
    [RelayCommand]
    public async Task AsignarHorarioAsync()
    {
        if (_medicoSeleccionado == null)
        {
            // Mostrar error o manejarlo según tu lógica
            Debug.WriteLine("No se ha seleccionado un médico.");
            return;
        }

        var nuevoHorario = new HorarioMedico
        {
            IdHorario = _medicoSeleccionado.Cedula, // Asegúrate de que esto sea lo que deseas como ID
            Fecha = _fecha,
            HoraInicio = _horaInicio,
            HoraFin = _horaFin
        };

        // Cargar todos los horarios del médico seleccionado
        var horariosExistentes = await _context.HorarioMedico
            .Where(h => h.IdHorario == nuevoHorario.IdHorario && h.Fecha == nuevoHorario.Fecha)
            .ToListAsync();

        // Validación de horarios para detectar conflictos
        var existeConflicto = horariosExistentes.Any(h =>
            (nuevoHorario.HoraInicio >= h.HoraInicio && nuevoHorario.HoraInicio < h.HoraFin) ||
            (nuevoHorario.HoraFin > h.HoraInicio && nuevoHorario.HoraFin <= h.HoraFin));

        if (existeConflicto)
        {
            Debug.WriteLine("Conflicto de horario detectado.");
            // Aquí puedes mostrar un mensaje al usuario o manejarlo según tu lógica
            return;
        }

        try
        {
            await _context.HorarioMedico.AddAsync(nuevoHorario);
            await _context.SaveChangesAsync();
            await CargarHorariosAsync(); // Recargar la lista después de guardar
        }
        catch (DbUpdateException ex)
        {
            Debug.WriteLine($"Error al guardar el horario: {ex.InnerException?.Message}");
            // Aquí podrías mostrar un mensaje al usuario o realizar otra acción
        }
    }

}
