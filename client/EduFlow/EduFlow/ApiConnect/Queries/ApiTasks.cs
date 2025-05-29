using EduFlow.ViewModels;
using EduFlowApi.DTOs.CourseDTOs;
using EduFlowApi.DTOs.TaskDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.ApiConnect
{
    public partial class ConnectionApi
    {
        public async Task<string> AddTask(AddTaskDTO newTask)
        {
            Client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);

            JsonContent newTaskSerialize = JsonContent.Create(newTask);

            HttpResponseMessage response = await Client.PostAsync("Task/AddTask", newTaskSerialize);

            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка добавления задачи!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateTask(UpdateTaskDTO updateTask)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            JsonContent updateTaskSerialize = JsonContent.Create(updateTask);
            HttpResponseMessage response = await Client.PutAsync("Task/UpdateTask", updateTaskSerialize);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка обновления задачи!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }

        public async Task<string> DeleteTask(Guid taskId)
        {
            Client.DefaultRequestHeaders.Authorization =
             new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.DeleteAsync($"Task/DeleteTask?taskId={taskId}");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка удаления задачи!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }
    }
}
