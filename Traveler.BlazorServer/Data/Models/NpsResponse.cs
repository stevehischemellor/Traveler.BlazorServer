using System.Text.Json.Serialization;

namespace Traveler.BlazorServer.Data.Models
{
    public class NpsResponse<T>
    {
        [JsonPropertyName("total")]
        public string? Total { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("limit")]
        public string? Limit { get; set; }

        [JsonPropertyName("start")]
        public string? Start { get; set; }
    }
}
