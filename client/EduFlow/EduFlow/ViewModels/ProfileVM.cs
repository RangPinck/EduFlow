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
            Profile = new SignInDTO()
            {
                Id = MainWindowViewModel.User.Id,
                Email = MainWindowViewModel.User.Email,
                UserSurname = MainWindowViewModel.User.UserSurname,
                UserName = MainWindowViewModel.User.UserName,
                UserPatronymic = MainWindowViewModel.User.UserPatronymic,
                IsFirst = MainWindowViewModel.User.IsFirst,
                UserRole = MainWindowViewModel.User.UserRole,
                Token = MainWindowViewModel.User.Token,
            };
            Profile.UserPatronymic = string.IsNullOrEmpty(Profile.UserPatronymic) ? "-" : Profile.UserPatronymic;
        }

        public void EditProfile()
        {
            MainWindowViewModel.Instance.RegistratePageBefore(nameof(Profile));
            MainWindowViewModel.Instance.PageContent = new EditProfile(MainWindowViewModel.User);
        }

        public async Task LogOut()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard(
                "Выход из аккаунта",
                "Вы действительно хотите выйти из аккаунта?",
                MsBox.Avalonia.Enums.ButtonEnum.YesNo,
                MsBox.Avalonia.Enums.Icon.Question).ShowAsync();

            if (result == ButtonResult.Yes)
            {
                MainWindowViewModel.Instance.IsAutorize = false;
                MainWindowViewModel.Instance.PageContent = new Login();
            }
        }

        public void GetUserStatistic()
        {
            MainWindowViewModel.Instance.RegistratePageBefore(nameof(Profile));
            MainWindowViewModel.Instance.PageContent = new UserStatistic(MainWindowViewModel.User.Id);
        }
    }
}
