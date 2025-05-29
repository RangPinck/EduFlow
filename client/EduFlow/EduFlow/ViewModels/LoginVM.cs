using Avalonia.Controls.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using EduFlow.ApiConnect;
using EduFlowApi.DTOs.AuthDTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            //**убрать
            {

                Email = "admin@admin.com",
                Password = "admin1cdbapi",
            };
        }

        private async Task CheckConnection()
        {
            try
            {
                MainWindowViewModel.Instance.IsOnline = await MainWindowViewModel.ApiClient.CheckAvailability() == HttpStatusCode.OK;

                if (!MainWindowViewModel.Instance.IsOnline)
                {
                    await MainWindowViewModel.ErrorMessage("Ошибка подключения!", "К сожалению, база данных не доступна. Повторите попытку позже.");
                }
            }
            catch
            {
                await MainWindowViewModel.ErrorMessage("Ошибка подключения!", "К сожалению, api не доступен. Повторите попытку позже.");
            }
        }

        public async Task Authorize()
        {
            try
            {
                MainWindowViewModel.ApiClient = new ConnectionApi("https://localhost:7053/api/");
                CheckConnection();

                var response = await MainWindowViewModel.ApiClient.LogInUser(LoginData);

                if (!string.IsNullOrEmpty(response))
                {
                    MainWindowViewModel.User = JsonConvert.DeserializeObject<SignInDTO>(response);

                    if (MainWindowViewModel.Instance.IsOnline)
                    {
                        if (MainWindowViewModel.User != null)
                        {
                            MainWindowViewModel.Instance.IsAutorize = true;

                            MainWindowViewModel.Instance.IsAdminKurator = new List<string>() { "Администратор", "Куратор" }
                                                                                            .Any(x => x == MainWindowViewModel.User.UserRole[0]);

                            MainWindowViewModel.Instance.IsAdmin = MainWindowViewModel.User.UserRole[0] == "Администратор";

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
