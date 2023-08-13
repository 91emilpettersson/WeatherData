using System.Text.Json.Serialization;

namespace WeatherData.Models
{
    /// <summary>
    /// Info about temperature, pressure and humidity
    /// </summary>
    public class MainInfo
    {
        /// <summary>
        /// Get and Set temperature
        /// </summary>
        [JsonPropertyName("temp")]
        public double? Temperature { get; set; }

        /// <summary>
        /// Get and Set pressure
        /// </summary>
        [JsonPropertyName("pressure")]
        public double? Pressure { get; set; }

        /// <summary>
        /// Get and Set humidity
        /// </summary
        [JsonPropertyName("humidity")]
        public double? Humidity { get; set; }
    }
}
