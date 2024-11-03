namespace clinicautp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new WelcomePage());
        }

        public void NavigateToAppShell()
        {
            // Luego de la bienvenida, cambia a AppShell
            MainPage = new AppShell();
        }
    }
}
