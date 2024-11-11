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

		MessagingCenter.Subscribe<HistorialMedicoViewModel, Cita>(this, "CitaCompletada", async (obj, cita) =>
        {
            //var citaCompletada = cita as CitaDTO;

            //DisplayAlert("Alert", "call back value is : " + cita.Id, "OK");
			//viewModel.ListaCitas.Remove(viewModel.ListaCitas.Where(c => c.Estado == "Completada").Single());
			await viewModel.ObtenerCitas();

        });

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