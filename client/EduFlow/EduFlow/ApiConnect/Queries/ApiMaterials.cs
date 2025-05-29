using EduFlow.ViewModels;
using EduFlowApi.DTOs.MaterialDTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EduFlow.ApiConnect
{
    public partial class ConnectionApi
    {
        public async Task<string> AddMaterial(AddMaterialDTO newMaterial)
        {
            Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);

            JsonContent newMaterialSerialize = JsonContent.Create(newMaterial);

            HttpResponseMessage response = await Client.PostAsync("Material/AddMaterial", newMaterialSerialize);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка добавления материала!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateMaterial(UpdateMaterialDTO updateMaterial)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            JsonContent updateMaterialSerialize = JsonContent.Create(updateMaterial);
            HttpResponseMessage response = await Client.PutAsync("Material/UpdateMaterial", updateMaterialSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка обновления материала!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> DeleteMaterial(Guid materialId)
        {
            Client.DefaultRequestHeaders.Authorization =
             new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.DeleteAsync($"Material/DeleteMaterial?materialId={materialId}");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка удаления материала!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> GetMaterialsTypes()
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.GetAsync("Material/GetMaterialsTypes");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка получения типов материалов!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }
    }
}
