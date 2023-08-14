using System;
using System.Text.Json;
using WeatherData.Models;

namespace WeatherData.Service.LocationClient
{
	internal class LocationClient : ILocationClient
	{
        private const string WeatherApiKey = "b60712a7abedfa03424432f6e5c1929f";

        private readonly HttpClient httpClient;
        private readonly ILogger<ILocationClient> logger;

		public LocationClient(HttpClient httpClient, ILogger<LocationClient> logger)
		{
			this.httpClient = httpClient;
            this.logger = logger;
		}


		public async Task<Location> GetLocationAsync(double latitude, double longitude, CancellationToken ct)
		{
            HttpResponseMessage locationResult = await httpClient.GetAsync($"http://api.openweathermap.org/geo/1.0/reverse?lat={latitude}&lon={longitude}&limit=1&appid={WeatherApiKey}", ct);

            if (!locationResult.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed getting location.");
                return null;
            }

            string locationJson = await locationResult.Content.ReadAsStringAsync(ct);

            List<Location> locations = JsonSerializer.Deserialize<List<Location>>(locationJson);

            if (locations == null)
            {
                logger.LogWarning("Failed getting location.");
                return null;
            }

            Location location = locations.FirstOrDefault();

            if (location == null)
            {
                logger.LogWarning("Failed getting location.");
                return null;
            }

            return location;
        }
    }
}

