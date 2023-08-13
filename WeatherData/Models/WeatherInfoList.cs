using System.Text.Json.Serialization;

namespace WeatherData.Models
{
    /// <summary>
    /// List of WeatherInfo
    /// </summary>
    public class WeatherInfoList
    {
        /// <summary>
        /// Creates a new instance of <see cref="WeatherInfoList"/>
        /// </summary>
        public WeatherInfoList()
        {
            List = new List<WeatherInfo>(); 
        }

        /// <summary>
        /// List of WeatherINfo
        /// </summary>
        [JsonPropertyName("list")]
        public List<WeatherInfo>? List { get; set; }
    }
}
