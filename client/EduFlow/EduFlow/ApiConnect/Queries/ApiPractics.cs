using EduFlow.ViewModels;
using EduFlowApi.DTOs.PracticeDTOs;
using EduFlowApi.DTOs.TaskDTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EduFlow.ApiConnect
{
    public partial class ConnectionApi
    {
        public async Task<string> AddPractice(AddPracticeDTO newPractice)
        {
            Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);

            JsonContent newPracticeSerialize = JsonContent.Create(newPractice);

            HttpResponseMessage response = await Client.PostAsync("Practice/AddPractice", newPracticeSerialize);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка добавления практики!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdatePractice(UpdatePracticeDTO updatePractice)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            JsonContent updatePracticeSerialize = JsonContent.Create(updatePractice);
            HttpResponseMessage response = await Client.PutAsync("Practice/UpdatePractice", updatePracticeSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка обновления практики!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> DeletePractice(Guid practiceId)
        {
            Client.DefaultRequestHeaders.Authorization =
             new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.DeleteAsync($"Practice/DeletePractice?practiceId={practiceId}");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка удаления практики!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }
    }
}
