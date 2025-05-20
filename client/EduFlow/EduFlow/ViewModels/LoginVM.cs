using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.AuthDTO;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class LoginVM : ViewModelBase
    {
        [ObservableProperty]
        private LoginDTO _loginData;

        public LoginVM()
        {
            LoginData = new LoginDTO()
            {
                Email = "admin@admin.com",
                Password = "admin1cdbapi",
            };
        }

        public async Task Authorize()
        {
            try
            {
                var response = await MainWindowViewModel.ApiClient.LogInUser(LoginData);

                if (!string.IsNullOrEmpty(response))
                {
                    MainWindowViewModel.User = JsonConvert.DeserializeObject<SignInDTO>(response);

                    if (MainWindowViewModel.Instance.IsOnline)
                    {
                        if (MainWindowViewModel.User != null)
                        {
                            MainWindowViewModel.Instance.IsAutorize = true;
                            MainWindowViewModel.Instance.PageContent = new CoursesPage();
                        }
                    }
                    else
                    {
                        await MainWindowViewModel.ErrorMessage("Нет ответа от АПИ!", "Нет соединения с АПИ! Попробуйте ещё раз позже");
                    }
                }
            }
            catch (Exception ex)
            {
                await MainWindowViewModel.ErrorMessage("Нет ответа от АПИ!", ex.Message);
            }
        }
    }
}
