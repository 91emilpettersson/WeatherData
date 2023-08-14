using System;
using WeatherData.Models;

namespace WeatherData.Service
{
	public interface IWeatherClient
	{
        public Task<WeatherInfoList> GetWeatherAsync(double latitude, double longitude, InformationType type, CancellationToken ct);
    }
}

