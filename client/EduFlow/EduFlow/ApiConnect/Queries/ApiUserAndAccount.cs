using EduFlow.ViewModels;
using EduFlowApi.DTOs.AccountDTOs;
using EduFlowApi.DTOs.AuthDTO;
using EduFlowApi.DTOs.CourseDTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EduFlow.ApiConnect
{
    public partial class ConnectionApi
    {
        public async Task<string> LogInUser(LoginDTO loginEnter)
        {
            JsonContent loginEnterSerialize = JsonContent.Create(loginEnter);

            HttpResponseMessage response = await Client.PostAsync("Account/Login", loginEnterSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка входа!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> GetAuthorizeUserData(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await Client.GetAsync("User/GetAuthorizeUserData");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> GetUserDataById(string token, Guid userId)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await Client.GetAsync($"User/GetUserDataById?userId={userId.ToString()}");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> GetAllUsers(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await Client.GetAsync("User/GetAllUsers");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка получения данных о пользователе!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> AddUser(RegistrationDTO newUser)
        {
            Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);

            JsonContent newUserSerialize = JsonContent.Create(newUser);

            HttpResponseMessage response = await Client.PostAsync("Account/Register", newUserSerialize);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка добавления пользователя!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateProfile(UpdateProfileDTO updateProfile)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            JsonContent newCourseSerialize = JsonContent.Create(updateProfile);
            HttpResponseMessage response = await Client.PutAsync("Account/UpdateProfile", newCourseSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка обновления профиля пользователя!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> UpdatePasswordUser(UpdatePasswordDTO newPassword)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            JsonContent newCourseSerialize = JsonContent.Create(newPassword);
            HttpResponseMessage response = await Client.PutAsync("Account/UpdatePasswordForProfile", newCourseSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка обновления пароля пользователя!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> UpdateRoleUser(UpdateUserRoleDTO newRole)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            JsonContent newCourseSerialize = JsonContent.Create(newRole);
            HttpResponseMessage response = await Client.PutAsync("Account/UpdateRoleProfile", newCourseSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка обновления роли пользователя!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> DeleteUser(Guid userId)
        {
            Client.DefaultRequestHeaders.Authorization =
             new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.DeleteAsync($"Account/DeleteAccount?userId={userId}");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка удаления аккаунта пользователя!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }
    }
}
