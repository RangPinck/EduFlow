using EduFlow.ViewModels;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EduFlow.ApiConnect
{
    public partial class ConnectionApi
    {
        public async Task<Stream> GetLogs(DateOnly date, string path)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", MainWindowViewModel.User.Token);
            HttpResponseMessage response = await Client.GetAsync($"Log/GetLogs?date={date}");

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Не удалось получить курсы!", response.Content.ToString());
            }

            var responseBody = await response.Content.ReadAsStreamAsync();

            return responseBody;
        }
    }
}
