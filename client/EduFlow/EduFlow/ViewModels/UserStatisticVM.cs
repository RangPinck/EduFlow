using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using EduFlow.Another.PDF;
using EduFlowApi.DTOs.UserDTOs;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class UserStatisticVM : ViewModelBase
    {
        [ObservableProperty]
        private UserDTO _userData;

        [ObservableProperty]
        private bool _hasData = false;

        [ObservableProperty]
        private string _savePath = @"" + Directory.GetCurrentDirectory();

        public UserStatisticVM() { }

        public UserStatisticVM(Guid userId)
        {
            if (userId == MainWindowViewModel.User.Id)
            {
                GetUserData();
            }
            else
            {
                GetUserDataById(userId);
            }
        }

        private async Task GetUserData()
        {
            var response = await MainWindowViewModel.ApiClient.GetAuthorizeUserData(MainWindowViewModel.User.Token);

            if (!string.IsNullOrEmpty(response))
            {
                var data = JsonConvert.DeserializeObject<UserDTO>(response);
                UserData = data;
            }

            HasData = _userData != null;
            OnPropertyChanged(nameof(UserData));
        }

        private async Task GetUserDataById(Guid userId)
        {
            var response = await MainWindowViewModel.ApiClient.GetUserDataById(MainWindowViewModel.User.Token, userId);

            if (!string.IsNullOrEmpty(response))
            {
                var data = JsonConvert.DeserializeObject<UserDTO>(response);
                UserData = data;
            }

            HasData = _userData != null;
            OnPropertyChanged(nameof(UserData));
        }

        public void GoToBack()
        {
            MainWindowViewModel.Instance.GoToPageBefore();
        }


        public async Task GetUserStatisticThatPDF()
        {
            await PdfStatistics.CreatePDF(UserData, SavePath);
        }
    }
}
