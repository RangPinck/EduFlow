using EduFlow.ViewModels;
using EduFlowApi.DTOs.BlockDTOs;
using EduFlowApi.DTOs.CourseDTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
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

        public async Task<string> AddBlock(AddBlockDTO newBlock)
        {
            Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);

            JsonContent newBlockSerialize = JsonContent.Create(newBlock);

            HttpResponseMessage response = await Client.PostAsync("Block/AddBlock", newBlockSerialize);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка добавления блока!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateBlock(UpdateBlockDTO updateBlock)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            JsonContent updateBlockSerialize = JsonContent.Create(updateBlock);
            HttpResponseMessage response = await Client.PutAsync("Block/UpdateBlock", updateBlockSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка обновления блока!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> DeleteBlock(Guid blockId)
        {
            Client.DefaultRequestHeaders.Authorization =
             new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.DeleteAsync($"Block/DeleteBlock?blockId={blockId}");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка удаления!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }
    }
}
