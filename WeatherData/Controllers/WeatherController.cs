using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WeatherData.Models;

namespace WeatherData.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> logger;
        private readonly HttpClient httpClient;

        //TODO: Put in Environment Variables
        private const string WeatherApiKey = "b60712a7abedfa03424432f6e5c1929f";
        private const string PremiumWeatherApiKey = "notAvailable";


        /// <summary>
        /// Creates a new instance of class <see cref="WeatherController"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="httpClient"></param>
        public WeatherController(
            ILogger<WeatherController> logger,
            HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Get info on weather on a certain coordinate.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<WeatherSummary>> Get(
            [FromQuery(Name = "lat")] double latitude,
            [FromQuery(Name = "lon")] double longitude,
            [FromQuery(Name = "type")] InformationType type,
            CancellationToken ct = default)
        {
            if(Math.Abs(latitude) > 90)
            {
                return BadRequest("Unallowed latitude");
            }

            if(Math.Abs(longitude) > 180)
            {
                return BadRequest("Unallowed longitude");
            }

            HttpResponseMessage locationResult = await httpClient.GetAsync($"http://api.openweathermap.org/geo/1.0/reverse?lat={latitude}&lon={longitude}&limit=1&appid={WeatherApiKey}", ct);

            // TODO: Extract to TryGet-method?
            if (!locationResult.IsSuccessStatusCode)
            {
                return StatusCode((int)locationResult.StatusCode, $"Call to api.openweathermap.org failed with the reason: {locationResult.ReasonPhrase}");
            }

            string locationJson = await locationResult.Content.ReadAsStringAsync(ct);

            List<Location> locations = JsonSerializer.Deserialize<List<Location>>(locationJson);

            if(locations == null)
            {
                return StatusCode(500, "Could not get location");
            }

            Location location = locations.FirstOrDefault();

            if(location == null) 
            {
                return StatusCode(500, "Could not get location");
            }

            WeatherInfoList weatherInfoList = new WeatherInfoList();

            if(type == InformationType.Current)
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={WeatherApiKey}";
                HttpResponseMessage result = await httpClient.GetAsync(url, ct);

                // TODO: Extract to TryGet-method?
                if (!result.IsSuccessStatusCode)
                {
                    return StatusCode((int)result.StatusCode, $"Call to api.openweathermap.org failed with the reason: {result.ReasonPhrase}");
                }

                string weatherJson = await result.Content.ReadAsStringAsync(ct);
                WeatherInfo weatherInfo = JsonSerializer.Deserialize<WeatherInfo>(weatherJson);

                // TODO: Extract to TryGet-method?
                if (weatherInfo == null)
                {
                    return StatusCode(500, "Could not get weather info");
                }

                weatherInfoList.List = new List<WeatherInfo>() { weatherInfo };
            }
            else if (type == InformationType.Forecast)
            {
                string url = $"https://pro.openweathermap.org/data/2.5/forecast/hourly?lat={latitude}&lon={longitude}&appid={PremiumWeatherApiKey}";
                HttpResponseMessage result = await httpClient.GetAsync(url, ct);

                // TODO: Extract to TryGet-method?
                if (!result.IsSuccessStatusCode)
                {
                    return StatusCode((int)result.StatusCode, $"Call to api.openweathermap.org failed with the reason: {result.ReasonPhrase}");
                }

                string weatherJson = await result.Content.ReadAsStringAsync(ct);
                weatherInfoList = JsonSerializer.Deserialize<WeatherInfoList>(weatherJson);

                // TODO: Extract to TryGet-method?
                if (weatherInfoList == null)
                {
                    return StatusCode(500, "Could not get weather info");
                }
            }
            else
            {
                logger.LogError("Unknown InformationType");
                throw new Exception($"Unknown type: {type}");
            }

            return new WeatherSummary(
                location,
                type,
                weatherInfoList);

        }





    }
}