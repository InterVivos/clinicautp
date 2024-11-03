using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using clinicautp.DataAccess;
using clinicautp.ViewModels;
using clinicautp.Views;

namespace clinicautp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Instanciar manualmente el contexto de base de datos
            var dbContext = new ClinicaDBContext();

            // Crear la base de datos si no existe
            dbContext.Database.EnsureCreated();

            // Liberar el recurso del contexto para evitar problemas de memoria
            dbContext.Dispose();

            // Registrar el contexto de base de datos para que esté disponible mediante inyección de dependencias
            builder.Services.AddDbContext<ClinicaDBContext>();

            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<LogInViewModel>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<PacienteViewModel>();
            builder.Services.AddTransient<HistorialMedicoViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<AdminViewModel>();
            builder.Services.AddTransient<AdminRegisterPacienteViewModel>();
            builder.Services.AddTransient<PacienteCitaViewModel>();
            builder.Services.AddTransient<CitaMainViewModel>();
            builder.Services.AddTransient<AdminPersonalMedicoMainViewModel>();
            builder .Services.AddTransient<AdminRegisterPersonalMedicoViewModel>(); 
            builder.Services.AddTransient<PersonalMedicoLoginViewModel>();
            builder.Services.AddTransient<PersonalMedicoMainViewModel> ();
            builder .Services.AddTransient<PersonalMedicoProfileViewModel> ();
            builder .Services.AddTransient<MedicamentoMainViewModel>();
            builder .Services.AddTransient<MedicamentoRegisterViewModel>();
            builder .Services.AddTransient<MedicamentoActualizarViewModel>();
            builder .Services.AddTransient<HorarioMedicoViewModel>();
            builder.Services.AddTransient<ReferenciaEspecialidadViewModel>();

            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<LogInPage>();
            builder.Services.AddTransient<HistorialMedicoPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<MainViewPage>();
            builder.Services.AddTransient<AdminPage>();
            builder.Services.AddTransient<AdminRegisterPacientePage>();
            builder.Services.AddTransient<PacienteCitaPage>();
            builder.Services.AddTransient<CitaMainViewPage>();
            builder.Services.AddTransient<AdminPersonalMedicoMainPage>();
            builder.Services.AddTransient<AdmRegisterPersonalMedicoPage>();
            builder.Services.AddTransient<PersonalMedicoLoginPage>();
            builder.Services.AddTransient<PersonalMedicoMainPage> ();
            builder.Services.AddTransient<PersonalMedicoProfilePage>();
            builder.Services.AddTransient<MedicamentoMainPage>();
            builder.Services.AddTransient<MedicamentoRegisterPage>();
            builder.Services.AddTransient<MedicamentoActualizarPage>();
            builder.Services.AddTransient<HorarioMedicoPage1>();
            builder.Services.AddTransient<ReferenciaEspecialidadPage>();

            Routing.RegisterRoute(nameof(HistorialMedicoPage), typeof(HistorialMedicoPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(MainViewPage), typeof(MainViewPage));
            Routing.RegisterRoute(nameof(AdminPage), typeof(AdminPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(AdminRegisterPacientePage), typeof(AdminRegisterPacientePage));
            Routing.RegisterRoute(nameof(PacienteCitaPage), typeof(PacienteCitaPage));
            Routing.RegisterRoute(nameof(CitaMainViewPage), typeof(CitaMainViewPage));
            Routing.RegisterRoute(nameof(AdminPersonalMedicoMainPage), typeof(AdminPersonalMedicoMainPage));
            Routing.RegisterRoute(nameof(AdmRegisterPersonalMedicoPage), typeof(AdmRegisterPersonalMedicoPage));
            Routing.RegisterRoute(nameof(PersonalMedicoMainPage), typeof(PersonalMedicoMainPage));
            Routing.RegisterRoute(nameof(PersonalMedicoProfilePage), typeof(PersonalMedicoProfilePage));
            Routing.RegisterRoute(nameof(MedicamentoMainPage), typeof(MedicamentoMainPage));
            Routing.RegisterRoute(nameof(LogInPage), typeof(LogInPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(MedicamentoRegisterPage), typeof(MedicamentoRegisterPage));
            Routing.RegisterRoute(nameof(MedicamentoActualizarPage), typeof(MedicamentoActualizarPage));
            Routing.RegisterRoute(nameof(HorarioMedicoPage1), typeof(HorarioMedicoPage1));
            Routing.RegisterRoute(nameof(ReferenciaEspecialidadPage), typeof(ReferenciaEspecialidadPage));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
