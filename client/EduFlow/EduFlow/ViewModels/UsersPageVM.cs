using CommunityToolkit.Mvvm.ComponentModel;
using EduFlowApi.DTOs.AuthDTO;
using EduFlowApi.DTOs.UserDTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class UsersPageVM : ViewModelBase
    {
        [ObservableProperty]
        private List<UserDTO> _users = new();

        [ObservableProperty]
        private bool _visibleList = true;

        public UsersPageVM()
        {
            GetUsers();
        }

        async Task GetUsers()
        {
            var response = await MainWindowViewModel.ApiClient.GetAllUsers(MainWindowViewModel.User.Token);

            if (!string.IsNullOrEmpty(response))
            {
                Users = JsonConvert.DeserializeObject<List<UserDTO>>(response);
            }
            else
            {
                VisibleList = false;
            }
        }

        public async Task UpdateUser(UserDTO user)
        {
            if (user is null)
            {
                await MainWindowViewModel.ErrorMessage("Пользователи",
                                                       "Для совершения действия выбурите пользователя, нажав на него!");
                return;
            }

            MainWindowViewModel.Instance.RegistratePageBefore(nameof(UserPage));

            MainWindowViewModel.Instance.PageContent = new EditProfile(new SignInDTO()
            {
                Id = user.UserId,
                Email = user.UserLogin,
                UserSurname = user.UserSurname,
                UserName = user.UserName,
                UserPatronymic = user.UserPatronymic,
                UserRole = user.UserRole,
                IsFirst = user.IsFirst

            });
        }

        public void AddUser()
        {
            MainWindowViewModel.Instance.RegistratePageBefore(nameof(UserPage));
            MainWindowViewModel.Instance.PageContent = new EditProfile();
        }

        public async Task GetStatisticUser(UserDTO user)
        {
            if (user is null)
            {
                await MainWindowViewModel.ErrorMessage("Пользователи", "Для совершения действия выбурите пользователя, нажав на него!");
                return;
            }

            MainWindowViewModel.Instance.RegistratePageBefore(nameof(UserPage));

            MainWindowViewModel.Instance.PageContent = new UserStatistic(user.UserId);
        }
    }
}
