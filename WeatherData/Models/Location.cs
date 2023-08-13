using System.Text.Json.Serialization;

namespace WeatherData.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Location
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("name")]
        public string? City { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }
    }
}
