using clinicautp.DTOs;
using clinicautp.Models;
using clinicautp.Utilities;
using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class PersonalMedicoMainPage : ContentPage
{
	public PersonalMedicoMainPage(PersonalMedicoMainViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

	private async void medicoCitasSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		//await Shell.Current.GoToAsync(nameof(ReferenciaEspecialidadPage));
		AppState.Instance.IdCitaSeleccionada = (e.CurrentSelection.FirstOrDefault() as CitaDTO).Id;
		var uri = $"{nameof(HistorialMedicoPage)}?id=0";
        await Shell.Current.GoToAsync(uri);
	}
}