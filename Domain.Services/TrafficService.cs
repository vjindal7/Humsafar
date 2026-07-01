using Contracts.Responses.Traffic;

using Microsoft.Extensions.Configuration;

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ITrafficService
    {
        Task<string> GetTrafficDetailsAsync(double sourceLat, double sourceLng, double destinationLat, double destinationLng, string travelMode = "car");
        Task<TrafficResponse> GetTrafficSummaryAsync(double sourceLat, double sourceLng, double destinationLat, double destinationLng, string travelMode = "car");
    }

    public class TrafficService : ITrafficService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TrafficService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetTrafficDetailsAsync(double sourceLat, double sourceLng, double destinationLat, double destinationLng, string travelMode = "car")
        {
            var apiKey = _configuration["Settings:TomTom:ApiKey"];
            var resolvedTravelMode = string.IsNullOrWhiteSpace(travelMode) ? "car" : travelMode.Trim().ToLowerInvariant();

            var url =
                $"https://api.tomtom.com/routing/1/calculateRoute/" +
                $"{sourceLat},{sourceLng}:{destinationLat},{destinationLng}/json" +
                $"?traffic=true" +
                $"&travelMode={resolvedTravelMode}" +
                $"&computeTravelTimeFor=all" +
                $"&key={apiKey}";

            return await _httpClient.GetStringAsync(url);
        }

        public async Task<TrafficResponse> GetTrafficSummaryAsync(double sourceLat, double sourceLng, double destinationLat, double destinationLng, string travelMode = "car")
        {
            var result = await GetTrafficDetailsAsync(sourceLat, sourceLng, destinationLat, destinationLng, travelMode);

            using var json = JsonDocument.Parse(result);

            var summary = json.RootElement
                .GetProperty("routes")[0]
                .GetProperty("summary");

            return new TrafficResponse
            {
                Distance = summary.GetProperty("lengthInMeters").GetInt32(),
                TravelTime = summary.GetProperty("travelTimeInSeconds").GetInt32(),
                TrafficDelay = summary.GetProperty("trafficDelayInSeconds").GetInt32(),
                NoTrafficTime = summary.GetProperty("noTrafficTravelTimeInSeconds").GetInt32(),
                ArrivalTime = summary.GetProperty("arrivalTime").GetString() ?? string.Empty
            };
        }
    }
}