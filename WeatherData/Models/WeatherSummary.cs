namespace WeatherData.Models
{
    public class WeatherSummary
    {
        /// <summary>
        /// Creates a new instance of class <see cref="WeatherSummary"/>
        /// </summary>
        /// <param name="location"></param>
        /// <param name="informationType"></param>
        /// <param name="weatherData"></param>
        public WeatherSummary(
            Location location,
            InformationType informationType,
            WeatherInfoList weatherInfoList)
        {
            Location = location;
            InformationType = informationType.ToString();
            WeatherInfoList = weatherInfoList;
        }

        /// <summary>
        /// The location the weather data relates to, e.g. a city name.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// The type of information, i.e. current weather or a weather forecast.
        /// </summary>
        public string InformationType { get; set; }

        /// <summary>
        /// The data describing the weather.
        /// </summary>
        public WeatherInfoList WeatherInfoList { get; set; }
    }
}


