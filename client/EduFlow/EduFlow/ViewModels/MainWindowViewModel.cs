using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using EduFlow.ApiConnect;
using EduFlowApi.DTOs.AuthDTO;
using EduFlowApi.DTOs.CourseDTOs;
using MsBox.Avalonia;
using System.Net;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _isOnline = false;

        [ObservableProperty]
        private UserControl _pageContent = new Login();

        [ObservableProperty]
        private UserControl _loginPage = new Login();

        [ObservableProperty]
        private ShortCourseDTO _selectedCourse = new ShortCourseDTO();

        [ObservableProperty]
        private bool _isAutorize = false;

        [ObservableProperty]
        private bool _isOpenMenu = false;

        [ObservableProperty]
        private bool _isAdminKurator = false;

        [ObservableProperty]
        private bool _isAdmin = false;

        private string _pageBefore = string.Empty;

        public static MainWindowViewModel Instance;

        public static SignInDTO User { get; set; } = null!;

        public static ConnectionApi ApiClient { get; set; }

        public MainWindowViewModel()
        {
            Instance = this;
        }

        public static async Task ErrorMessage(string title, string message)
        {
            await MessageBoxManager.GetMessageBoxStandard(title, message, MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
        }

        public void ChengeMenuState()
        {
            IsOpenMenu = !IsOpenMenu;
        }

        public void GoToUserPage()
        {
            PageContent = new UserPage();
        }

        public void GoToProfile()
        {
            PageContent = new Profile();
        }

        public void GoToCoursesPage()
        {
            PageContent = new CoursesPage();
        }

        public void GoToLogsPage()
        {
            PageContent = new LogsPage();
        }

        public void RegistratePageBefore(string nameControl)
        {
            _pageBefore = nameControl;
        }

        public void GoToPageBefore()
        {
            PageContent = _pageBefore switch
            {
                nameof(BlokPage) => new BlokPage(course: SelectedCourse),
                nameof(CoursesPage) => new CoursesPage(),
                nameof(Login) => new Login(),
                nameof(Profile) => new Profile(),
                nameof(UserPage) => new UserPage(),
            };
        }
    }
}
