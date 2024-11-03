using System.Collections.ObjectModel;
using System.Threading.Tasks;
using clinicautp.Models;
using clinicautp.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using clinicautp.Views;
using clinicautp.Utilities;

namespace clinicautp.ViewModels
{
    public partial class AdminPersonalMedicoMainViewModel : ObservableObject
    {
        private readonly ClinicaDBContext _dbContext;

        // Propiedad que almacena la lista de cédulas de personal médico y notifica cambios en la vista
        [ObservableProperty]
        private ObservableCollection<string> listaCedulasPersonalMedico = new ObservableCollection<string>();

        
        public AdminPersonalMedicoMainViewModel(ClinicaDBContext context)
        {
            _dbContext = context;
            
            MainThread.BeginInvokeOnMainThread(async () => await LoadPersonalMedico());
        }

       
        public async Task LoadPersonalMedico()
        {
            
            ListaCedulasPersonalMedico.Clear();

            
            var lista = await _dbContext.PersonalMedicos
                .Select(pm => pm.Cedula)
                .ToListAsync();

            if (lista.Any())
            {
                foreach (var cedula in lista)
                {
                    ListaCedulasPersonalMedico.Add(cedula);
                }
            }
        }

        // Comando para agregar un nuevo personal médico
        [RelayCommand]
        private async Task AgregarPersonalM()
        {
            var uri = nameof(AdmRegisterPersonalMedicoPage);
            await Shell.Current.GoToAsync(uri);
        }

        // Comando para actualizar los datos de un personal médico
        [RelayCommand]
        private async Task ActualizarPersonalM(string cedula)
        {
            AppState.Instance.CedulaPersonalMedico = cedula;

            var profileViewModel = new PersonalMedicoProfileViewModel(_dbContext);
            await profileViewModel.LoadPersonalMedicoAsync(); 

            var uri = $"{nameof(PersonalMedicoProfilePage)}?cedula={cedula}";
            await Shell.Current.GoToAsync(uri);
        }


        [RelayCommand]
        private async Task EliminarPersonalM(string cedula)
        {
  
            var personalMedico = await _dbContext.PersonalMedicos
                .FirstOrDefaultAsync(pm => pm.Cedula == cedula);

            if (personalMedico != null)
            {

                _dbContext.PersonalMedicos.Remove(personalMedico);
                await _dbContext.SaveChangesAsync();


                await LoadPersonalMedico();

                await Application.Current.MainPage.DisplayAlert("Éxito", "Personal médico eliminado correctamente.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se encontró al personal médico.", "OK");
            }
        }

    }
}
