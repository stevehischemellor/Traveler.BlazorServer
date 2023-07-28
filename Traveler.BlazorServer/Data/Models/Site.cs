using System.Text.Json.Serialization;

namespace Traveler.BlazorServer.Data.Models
{
    public class Site
    {
        [JsonPropertyName("states")]
        public string? States { get; set; }

        [JsonPropertyName("parkCode")]
        public string? ParkCode { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("latitude")]
        public string? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public string? Longitude { get; set; }
    }
}
