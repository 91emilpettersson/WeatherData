using System.Text.Json.Serialization;

namespace WeatherData.Models
{
    /// <summary>
    /// Main weather, e.g. Cloudy, Sunny, Rainy
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// Main weather, e.g. Cloudy, Sunny, Rainy
        /// </summary>
        [JsonPropertyName("main")]
        public string? MainWeather { get; set; }
    }
}
