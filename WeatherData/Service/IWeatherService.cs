using System;
using WeatherData.Models;

namespace WeatherData.Service
{
	public interface IWeatherService
	{
        public Task<WeatherSummary> GetWeatherSummaryAsync(double latitude, double longitude, InformationType type, CancellationToken ct);
    }
}

