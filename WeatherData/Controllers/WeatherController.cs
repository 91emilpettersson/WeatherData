using Microsoft.AspNetCore.Mvc;
using WeatherData.Models;
using WeatherData.Service;

namespace WeatherData.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService weatherService;
        private readonly ILogger<WeatherController> logger;

        /// <summary>
        /// Creates a new instance of class <see cref="WeatherController"/>
        /// </summary>
        /// <param name="logger"></param>
        public WeatherController(
            IWeatherService weatherService,
            ILogger<WeatherController> logger)
        {
            this.weatherService = weatherService;
            this.logger = logger;
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

            WeatherSummary weatherSummary = await weatherService.GetWeatherSummaryAsync(latitude, longitude, type, ct);

            if (weatherSummary == null)
            {
                logger.LogWarning($"Could not get weather for latitude: {latitude}, longitutde: {longitude}, information type: {type}");
                return StatusCode(500, $"Could not get weather for latitude: {latitude}, longitutde: {longitude}, information type: {type}");
            }

            return weatherSummary;
        }
    }
}