using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.AuthDTO;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class ProfileVM : ViewModelBase
    {
        [ObservableProperty]
        private SignInDTO _profile;

        public ProfileVM()
        {
            Profile = MainWindowViewModel.User;
            Profile.UserPatronymic = string.IsNullOrEmpty(Profile.UserPatronymic) ? "-" : Profile.UserPatronymic;
        }

        public void EditProfile()
        {
            MainWindowViewModel.Instance.PageContent = new EditProfile(MainWindowViewModel.User, new Profile());
        }

        public async Task LogOut()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard("Выход из аккаунта", "Вы действительно хотите выйти из аккаунта?", MsBox.Avalonia.Enums.ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Question).ShowAsync();

            if (result == ButtonResult.Yes)
            {
                MainWindowViewModel.Instance.IsAutorize = false;
                MainWindowViewModel.Instance.PageContent = new Login();
            }
        }
    }
}
