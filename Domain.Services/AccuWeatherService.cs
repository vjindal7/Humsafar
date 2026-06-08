using Microsoft.Extensions.Configuration;

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using View.Models;

namespace Domain.Services
{
    public interface IAccuWeatherService
    {
    }

    public class AccuWeatherService : IAccuWeatherService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public AccuWeatherService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        private string ApiKey => _config["AccuWeather:ApiKey"]!;

        public async Task<string> GetLocationKeyAsync(string city)
        {
            var url = $"https://dataservice.accuweather.com/locations/v1/cities/search" +
                      $"?apikey={ApiKey}&q={city}";

            var json = await _http.GetStringAsync(url);
            var doc = JsonDocument.Parse(json);

            return doc.RootElement[0].GetProperty("Key").GetString()!;
        }

        public async Task<WeatherViewModel> GetCurrentWeatherAsync(string city)
        {
            var key = await GetLocationKeyAsync(city);

            var url = $"https://dataservice.accuweather.com/currentconditions/v1/{key}" +
                      $"?apikey={ApiKey}&details=true";

            var json = await _http.GetStringAsync(url);
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement[0];

            return new WeatherViewModel
            {
                City = city,
                Condition = root.GetProperty("WeatherText").GetString()!,
                TemperatureC = root.GetProperty("Temperature")
                                    .GetProperty("Metric")
                                    .GetProperty("Value").GetDecimal()
            };
        }
    }
}
