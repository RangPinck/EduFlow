using EduFlow.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace EduFlow.ApiConnect
{
    public partial class ConnectionApi
    {
        public async Task<string> GetAllRoles(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await Client.GetAsync("Role/GetAllRoles");
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                await MainWindowViewModel.ErrorMessage("Ошибка получения данных о ролях!", ParseErrorResponse(responseBody));
                return string.Empty;
            }

            return responseBody;
        }


    }
}
