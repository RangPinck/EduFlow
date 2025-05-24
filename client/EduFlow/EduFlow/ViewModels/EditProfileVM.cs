using CommunityToolkit.Mvvm.ComponentModel;
using EduFlow.Models.Account;
using EduFlowApi.DTOs.AccountDTOs;
using EduFlowApi.DTOs.AuthDTO;
using EduFlowApi.DTOs.RoleDTOs;
using EduFlowApi.DTOs.UserDTOs;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduFlow.ViewModels
{
    public partial class EditProfileVM : ViewModelBase
    {
        [ObservableProperty]
        private string _header = "";

        [ObservableProperty]
        private bool _isCreateUser = false;

        [ObservableProperty]
        private bool _isAdmin = false;

        [ObservableProperty]
        private RoleDTO _selectedRole;

        [ObservableProperty]
        private bool _deleteView = false;

        [ObservableProperty]
        private List<RoleDTO> _roles;

        [ObservableProperty]
        private FullUserDTO _fullUser;

        [ObservableProperty]
        private RegistrationDTO _workedUser;

        public EditProfileVM(SignInDTO user)
        {
            Header = "Измененение пользователя";

            FullUser = new FullUserDTO()
            {
                UserId = user.Id,
                Email = user.Email,
                UserSurname = user.UserSurname,
                UserName = user.UserName,
                UserPatronymic = user.UserPatronymic,
                IsFirst = user.IsFirst,
            };

            IsAdmin = MainWindowViewModel.Instance.IsAdmin && MainWindowViewModel.User.Id != user.Id;

            DeleteView = MainWindowViewModel.Instance.IsAdminKurator && MainWindowViewModel.User.Id != user.Id;

            GetAllRoles(user);
        }

        public EditProfileVM()
        {
            GetAllRoles(null);
            Header = "Создание пользователя";
            IsCreateUser = true;

            FullUser = new FullUserDTO();

            SelectedRole = new RoleDTO() { RoleId = new Guid(), RoleName = "Не выбрано" };
        }

        public async Task GoToBack()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard(Header, "Вы действительно хотите покинуть страницу? Произведённые изменения не сохраняться!", MsBox.Avalonia.Enums.ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Question).ShowAsync();

            if (result == ButtonResult.Yes)
            {
                MainWindowViewModel.Instance.GoToPageBefore();
            }
        }

        public async Task GetAllRoles(SignInDTO user)
        {
            var response = await MainWindowViewModel.ApiClient.GetAllRoles(MainWindowViewModel.User.Token);

            if (!string.IsNullOrEmpty(response))
            {
                if (MainWindowViewModel.Instance.IsAdmin)
                {
                    Roles = JsonConvert.DeserializeObject<List<RoleDTO>>(response).ToList();

                    Roles = [new RoleDTO() { RoleId = new Guid(), RoleName = "Не выбрано" }, .. Roles];

                    if (!IsCreateUser)
                    {
                        SelectedRole = Roles.First(x => x.RoleName == user.UserRole[0]);
                    }
                }
                else
                {
                    Roles = JsonConvert.DeserializeObject<List<RoleDTO>>(response).ToList();
                    Roles = Roles.Where(x => x.RoleName.Contains("Ученик")).ToList();
                }
            }
        }

        public async Task DeleteUser()
        {
            var result = await MessageBoxManager.GetMessageBoxStandard("Удаление пользователя", $"Вы действительно хотите удалить  {FullUser.UserSurname + " " + FullUser.UserName}?", MsBox.Avalonia.Enums.ButtonEnum.YesNo).ShowAsync();


            if (result == MsBox.Avalonia.Enums.ButtonResult.Yes)
            {
                string response = await MainWindowViewModel.ApiClient.DeleteUser(FullUser.UserId);

                if (string.IsNullOrEmpty(response))
                {
                    return;
                }
            }

            MainWindowViewModel.Instance.GoToPageBefore();
        }

        public async Task SaveUserData()
        {

            if (SelectedRole != null && SelectedRole.RoleName == "Не выбрано")
            {
                await MainWindowViewModel.ErrorMessage("Создание пользователя", "Роль пользователя не выбрана!");

                return;
            }

            string response = string.Empty;

            if (IsCreateUser)
            {
                response = await MainWindowViewModel.ApiClient.AddUser(new RegistrationDTO()
                {
                    IsFirst = true,
                    UserName = FullUser.UserName,
                    UserPatronymic = FullUser.UserPatronymic,
                    UserSurname = FullUser.UserSurname,
                    Email = FullUser.Email,
                    Password = FullUser.Password,
                    ConfirmPassword = FullUser.ConfirmPassword,
                    RoleId = SelectedRole.RoleId,
                });

                if (string.IsNullOrEmpty(response))
                {
                    return;
                }
            }
            else
            {
                response = await MainWindowViewModel.ApiClient.UpdateProfile(new UpdateProfileDTO()
                {
                    UserId = FullUser.UserId,
                    UserSurname = FullUser.UserSurname,
                    Email = FullUser.Email,
                    UserName = FullUser.UserName,
                    UserPatronymic = FullUser.UserPatronymic
                });

                if (string.IsNullOrEmpty(response))
                {
                    return;
                }

                if (!string.IsNullOrEmpty(FullUser.Password) || !string.IsNullOrEmpty(FullUser.ConfirmPassword))
                {
                    response = await MainWindowViewModel.ApiClient.UpdatePasswordUser(new UpdatePasswordDTO()
                    {
                        Email = FullUser.Email,
                        Password = FullUser.Password,
                        ConfirmPassword = FullUser.ConfirmPassword
                    });

                    if (string.IsNullOrEmpty(response))
                    {
                        return;
                    }
                }

                if (IsAdmin)
                {
                    response = await MainWindowViewModel.ApiClient.UpdateRoleUser(new UpdateUserRoleDTO()
                    {
                        UserId = FullUser.UserId,
                        RoleId = SelectedRole.RoleId,
                    });

                    if (string.IsNullOrEmpty(response))
                    {
                        return;
                    }
                }
            }

            MainWindowViewModel.Instance.GoToPageBefore();
        }
    }
}
