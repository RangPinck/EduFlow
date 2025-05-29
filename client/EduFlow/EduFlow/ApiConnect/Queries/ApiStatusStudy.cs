using EduFlow.ViewModels;
using EduFlowApi.DTOs.CourseDTOs;
using EduFlowApi.DTOs.StudyStateDTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EduFlow.ApiConnect
{
    public partial class ConnectionApi
    {
        public async Task<string> GetStudyStatmens()
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.GetAsync("StatusStudy/GetStudyStatesAsync");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка получения данных о стадиях изучения!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> UpdateStatus(UpdateStudyStateDTO updateStudy)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            JsonContent updateStudySerialize = JsonContent.Create(updateStudy);
            HttpResponseMessage response = await Client.PutAsync("StatusStudy/UpdateStudyState", updateStudySerialize);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка обновления статуса!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }
    }
}
