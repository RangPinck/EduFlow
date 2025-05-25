using EduFlow.ViewModels;
using EduFlowApi.DTOs.CourseDTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EduFlow.ApiConnect
{
    public partial class ConnectionApi
    {
        public async Task<string> GetAllCourses()
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

        public async Task<string> GetFullCoursData(Guid courseId, Guid userId)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.GetAsync($"Course/GetCourseById?courseId={courseId}&userId={userId}");

            string responseBody = await response.Content.ReadAsStringAsync();

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
                return string.Empty;
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

        public async Task<string> DeleteCourse(Guid courseId)
        {
            Client.DefaultRequestHeaders.Authorization =
             new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.DeleteAsync($"Course/DeleteCourse?courseId={courseId}");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка обновления курса!", ParseErrorResponse(responseBody));
            }

            return responseBody;
        }
    }
}
