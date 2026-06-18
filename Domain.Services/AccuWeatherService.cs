using Contracts.Responses.Weather;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IAccuWeatherService
    {
        Task<WeatherResponse> GetCurrentWeatherAsync(string city);
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
            var url = $"https://dataservice.accuweather.com/locations/v1/cities/search" + $"?apikey={ApiKey}&q={city}";

            var json = await _http.GetStringAsync(url);
            var doc = JsonDocument.Parse(json);

            return doc.RootElement[0].GetProperty("Key").GetString()!;
        }

        public async Task<WeatherResponse> GetCurrentWeatherAsync(string city)
        {
            var key = await GetLocationKeyAsync(city);

            var url = $"https://dataservice.accuweather.com/currentconditions/v1/{key}" + $"?apikey={ApiKey}&details=true";

            var json = await _http.GetStringAsync(url);
            var doc = JsonDocument.Parse(json);
            var weather = doc.RootElement[0];

            return new WeatherResponse
            {
                City = city,

                Condition = weather
                    .GetProperty("WeatherText")
                    .GetString(),

                TemperatureC = weather
                    .GetProperty("Temperature")
                    .GetProperty("Metric")
                    .GetProperty("Value")
                    .GetDecimal()
            };
        }
    }
}
