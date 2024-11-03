namespace clinicautp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Navegar a la página de registro
                await Shell.Current.GoToAsync("RegisterPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al navegar: {ex.Message}", "OK");
            }
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Navegar a la página de inicio de sesión
                await Shell.Current.GoToAsync("LogInPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al navegar: {ex.Message}", "OK");
            }
        }

    }
}
