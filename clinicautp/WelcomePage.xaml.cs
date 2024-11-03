namespace clinicautp;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
        InitializeComponent();

        // NavigationPage para ocultar barra de inicio
        NavigationPage.SetHasNavigationBar(this, false);
    }


    // Evento OnClicked para ir a la siguiente p�gina
    private async void OnCounterClicked(object sender, EventArgs e)
    {
        // Cambia la p�gina principal a AppShell, que contiene la barra lateral
        Application.Current.MainPage = new AppShell();
    }
}