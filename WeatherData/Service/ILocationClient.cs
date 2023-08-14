using System;
using WeatherData.Models;

namespace WeatherData.Service
{
	public interface ILocationClient
	{
        public Task<Location> GetLocationAsync(double latitude, double longitude, CancellationToken ct);
    }
}

