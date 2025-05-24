using EduFlow.Models.ErrorDTO;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EduFlow.ApiConnect
{
    public partial class ConnectionApi
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
                if (errorResponse != null && errorResponse.Errors != null)
                {
                    foreach (var error in errorResponse.Errors)
                    {
                        errorMessage.AppendLine($"{string.Join(", ", error.Value)}");
                    }
                }

                return errorResponse != null && errorMessage.Length > 0 ? errorMessage.ToString() : errorResponse.Title;
            }
            catch
            {
                return jsonResponse;
            }
        }
    }
}
