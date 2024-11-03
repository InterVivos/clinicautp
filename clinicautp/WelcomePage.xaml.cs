namespace clinicautp;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
        InitializeComponent();

        // NavigationPage para ocultar barra de inicio
        NavigationPage.SetHasNavigationBar(this, false);
    }


    // Evento OnClicked para ir a la siguiente página
    private async void OnCounterClicked(object sender, EventArgs e)
    {
        // Cambia la página principal a AppShell, que contiene la barra lateral
        Application.Current.MainPage = new AppShell();
    }
}