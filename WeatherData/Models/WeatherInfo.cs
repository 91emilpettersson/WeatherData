using System.Text.Json.Serialization;

namespace WeatherData.Models
{
    /// <summary>
    /// Info about weather
    /// </summary>
    public class WeatherInfo
    {
        /// <summary>
        /// List of weather
        /// </summary>
        [JsonPropertyName("weather")]
        public List<Weather>? Weather { get; set; }
                
        /// <summary>
        /// Main weather info
        /// </summary>
        [JsonPropertyName("main")]
        public MainInfo? MainInfo { get; set; }
            
        /// <summary>
        /// Info about wind
        /// </summary>
        [JsonPropertyName("wind")]
        public Wind? Wind { get; set; }
                
        /// <summary>
        /// Date correlating to the info
        /// </summary>
        [JsonPropertyName("dt_txt")]
        public string? Date { get; set; }
    }
}
