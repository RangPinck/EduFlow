using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EduFlow.Models.ErrorDTO
{
    public class ApiErrorResponse
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, List<string>> Errors { get; set; }
    }
}
