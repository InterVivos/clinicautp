using clinicautp.ViewModels;

namespace clinicautp.Views;

[QueryProperty(nameof(idHistorialMedico), "id")]
public partial class HistorialMedicoPage : ContentPage
{
	public int idHistorialMedico
	{
		set
		{
			
		}
	}

	public HistorialMedicoPage(HistorialMedicoViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}