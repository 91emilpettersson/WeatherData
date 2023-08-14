using System;
using WeatherData.Models;

namespace WeatherData.Service.WeatherService
{
	internal class WeatherService : IWeatherService
	{
        private readonly ILocationClient locationClient;
	    private readonly IWeatherClient weatherClient;
		private readonly ILogger<WeatherService> logger;

		public WeatherService(
			ILocationClient locationClient,
			IWeatherClient weatherClient,
			ILogger<WeatherService> logger)
		{
            this.locationClient = locationClient;
            this.weatherClient = weatherClient;
			this.logger = logger;
        }

		public async Task<WeatherSummary> GetWeatherSummaryAsync(double latitude, double longitude, InformationType type, CancellationToken ct)
		{
			Location location = await locationClient.GetLocationAsync(latitude, longitude, ct);

			if(location == null)
			{
				logger.LogWarning($"Could not get location for latitude: {latitude}, longitutde: {longitude}");
				return null;
			}

			WeatherInfoList weatherInfoList = await weatherClient.GetWeatherAsync(latitude, longitude, type, ct);

			if(weatherInfoList == null)
			{
                logger.LogWarning($"Could not get weather for latitude: {latitude}, longitutde: {longitude}, information type: {type}");
                return null;
			}

			return new WeatherSummary(
                location,
                type,
                weatherInfoList);
        }
	}
}

