using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.AuthDTO;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class EditProfileVM : ViewModelBase
    {
        [ObservableProperty]
        private string _header = "";

        private UserControl _latesPage;

        [ObservableProperty]
        private bool _isCreateUser = false;

        [ObservableProperty]
        private RegistrationDTO _workedUser;

        public EditProfileVM(SignInDTO user, UserControl latestPage)
        {
            _latesPage = latestPage;
            Header = "Измененение пользователя";
        }

        public EditProfileVM(UserControl latestPage)
        {
            _latesPage = latestPage;
            Header = "Создание пользователя";
            IsCreateUser = true;
        }

        public EditProfileVM() { }

        public async Task GoToBack()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard(Header, "Вы действительно хотите покинуть страницу? Произведённые изменения не сохраняться!", MsBox.Avalonia.Enums.ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Question).ShowAsync();

            if (result == ButtonResult.Yes)
            {
                MainWindowViewModel.Instance.PageContent = _latesPage;
            }
        }
    }
}
