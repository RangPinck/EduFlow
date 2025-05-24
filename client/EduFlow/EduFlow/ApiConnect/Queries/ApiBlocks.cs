using EduFlow.ViewModels;
using EduFlowApi.DTOs.CourseDTOs;
using System.Net.Http;
using System.Threading.Tasks;

namespace EduFlow.ApiConnect
{
    public partial class ConnectionApi
    {
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
