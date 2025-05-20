using Avalonia.Controls;
using Avalonia.Controls.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using EduFlow.ApiConnect;
using EduFlow.Views;
using EduFlowApi.DTOs.AuthDTO;
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
        private bool _isAutorize = false;

        [ObservableProperty]
        private bool _isOpenMenu = false;

        public static MainWindowViewModel Instance;

        public static SignInDTO User { get; set; } = null!;

        public static ConnectionApi ApiClient { get; set; }

        public MainWindowViewModel()
        {
            Instance = this;
            ApiClient = new ConnectionApi("https://localhost:7053/api/");
            CheckConnection();
        }

        public static async Task ErrorMessage(string title, string message)
        {
            await MessageBoxManager.GetMessageBoxStandard(title, message, MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
        }

        private async Task CheckConnection()
        {
            try
            {
                IsOnline = await ApiClient.CheckAvailability() == HttpStatusCode.OK;

                if (!_isOnline)
                {
                    await ErrorMessage("Ошибка подключения!", "К сожалению, база данных не доступна. Повторите попытку позже.");
                }
            }
            catch
            {
                await ErrorMessage("Ошибка подключения!", "К сожалению, api не доступен. Повторите попытку позже.");
            }
        }

        public void ChengeMenuState()
        {
            IsOpenMenu = !IsOpenMenu;
        }
    }
}
