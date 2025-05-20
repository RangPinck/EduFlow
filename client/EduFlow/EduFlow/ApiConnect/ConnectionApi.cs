using EduFlow.Models.ErrorDTO;
using EduFlow.ViewModels;
using EduFlowApi.DTOs.AuthDTO;
using EduFlowApi.DTOs.CourseDTOs;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.ApiConnect
{
    public class ConnectionApi
    {
        private HttpClient _client;

        public ConnectionApi(string baseUri)
        {
            Client = new HttpClient();
            Client.BaseAddress = new System.Uri(baseUri);
        }

        public HttpClient Client
        {
            get => _client;
            set => _client = value;
        }

        public async Task<HttpStatusCode> CheckAvailability()
        {
            HttpResponseMessage response = await Client.GetAsync("Connection/CheckConnection");
            return response.StatusCode;
        }

        private string ParseErrorResponse(string jsonResponse)
        {
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<ApiErrorResponse>(jsonResponse);

                var errorMessage = new StringBuilder();
                if (errorResponse.Errors != null)
                {
                    foreach (var error in errorResponse.Errors)
                    {
                        errorMessage.AppendLine($"{string.Join(", ", error.Value)}");
                    }
                }
                return errorMessage.Length > 0 ? errorMessage.ToString() : errorResponse.Title;
            }
            catch
            {
                return jsonResponse;
            }
        }

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

        public async Task<string> GetAllUsers(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await Client.GetAsync("User/GetAllUsers");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Не удалось получить пользователей!", response.Content.ToString());
            }

            return responseBody;
        }

        public async Task<string> GetAllCourses(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.GetAsync("Course/GetAllCourses");

            string responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        public async Task<string> GetCourses()
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.GetAsync("Course/GetAuthorsForCourses");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Не удалось получить курсы!", response.Content.ToString());
            }

            return responseBody;
        }

        public async Task<string> AddCourse(AddCourseDTO newCourse)
        {
            Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            JsonContent newCourseSerialize = JsonContent.Create(newCourse);
            HttpResponseMessage response = await Client.PostAsync("Course/AddCourse", newCourseSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка добавления курса!", ParseErrorResponse(responseBody));
            }


            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateCourse(UpdateCourseDTO newCourse)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            JsonContent newCourseSerialize = JsonContent.Create(newCourse);
            HttpResponseMessage response = await Client.PutAsync("Course/UpdateCourse", newCourseSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка обновления курса!", ParseErrorResponse(responseBody));
            }

            return responseBody;
        }

        public async Task<string> DeleteCourse(ShortCourseDTO courseInfo)
        {
            Client.DefaultRequestHeaders.Authorization =
             new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.DeleteAsync($"Course/DeleteCourse?courseId={courseInfo.CourseId}");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка обновления курса!", ParseErrorResponse(responseBody));
            }

            return responseBody;
        }

        public async Task<string> GetBlock(ShortCourseDTO courseInfo)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.GetAsync($"Block/GetBlockOfCourse?courseId={courseInfo.CourseId}");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка получения блока!", response.Content.ToString());
            }

            return responseBody;
        }
    }
}
