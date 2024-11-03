using clinicautp.ViewModels;

namespace clinicautp.Views;

public partial class ReferenciaEspecialidadPage : ContentPage
{
	public ReferenciaEspecialidadPage(ReferenciaEspecialidadViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}