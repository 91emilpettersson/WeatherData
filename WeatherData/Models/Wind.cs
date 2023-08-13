using System.Text.Json.Serialization;

namespace WeatherData.Models
{
    /// <summary>
    /// Info about wind
    /// </summary>
    public class Wind
    {
        /// <summary>
        /// The speed of the wind
        /// </summary>
        [JsonPropertyName("speed")]
        public double? Speed { get; set; }
    }
}
