using System;
using System.Text.Json;
using WeatherData.Models;

namespace WeatherData.Service.WeatherClient
{
	internal class WeatherClient : IWeatherClient
	{
        private const string WeatherApiKey = "b60712a7abedfa03424432f6e5c1929f";
        private const string PremiumWeatherApiKey = "notAvailable";

        private readonly HttpClient httpClient;
        private readonly ILogger<WeatherClient> logger;

		public WeatherClient(HttpClient httpClient, ILogger<WeatherClient> logger)
		{
			this.httpClient = httpClient;
            this.logger = logger;
		}

        public async Task<WeatherInfoList> GetWeatherAsync(double latitude, double longitude, InformationType type, CancellationToken ct)
        {
            WeatherInfoList weatherInfoList = new WeatherInfoList();

            if (type == InformationType.Current)
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={WeatherApiKey}";
                HttpResponseMessage result = await httpClient.GetAsync(url, ct);

                // TODO: Extract to TryGet-method?
                if (!result.IsSuccessStatusCode)
                {
                    logger.LogWarning($"Call to api.openweathermap.org failed with the reason: {result.ReasonPhrase}");
                    return null;
                }

                string weatherJson = await result.Content.ReadAsStringAsync(ct);
                WeatherInfo weatherInfo = JsonSerializer.Deserialize<WeatherInfo>(weatherJson);

                // TODO: Extract to TryGet-method?
                if (weatherInfo == null)
                {
                    logger.LogWarning($"Call to api.openweathermap.org failed with the reason: {result.ReasonPhrase}");
                    return null;
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
                    logger.LogWarning($"Call to api.openweathermap.org failed with the reason: {result.ReasonPhrase}");
                    return null;
                }

                string weatherJson = await result.Content.ReadAsStringAsync(ct);
                weatherInfoList = JsonSerializer.Deserialize<WeatherInfoList>(weatherJson);

                // TODO: Extract to TryGet-method?
                if (weatherInfoList == null)
                {
                    logger.LogWarning($"Call to api.openweathermap.org failed with the reason: {result.ReasonPhrase}");
                    return null;
                }
            }
            else
            {
                logger.LogError("Unknown InformationType");
                throw new Exception($"Unknown type: {type}");
            }

            return weatherInfoList;
        }
    }
}

