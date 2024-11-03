namespace clinicautp.Views;

public partial class HorarioMedicoPage1 : ContentPage
{
	public HorarioMedicoPage1(HorarioMedicoViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}