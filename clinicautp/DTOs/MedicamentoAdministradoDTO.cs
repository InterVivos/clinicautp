using clinicautp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace clinicautp.DTOs;

public partial class MedicamentoAdministradoDTO : ObservableObject
{
    public int IdHistorial { get; set; }

    public Medicamento Medicamento { get; set; }

    [ObservableProperty]
    public int cantidad;
}