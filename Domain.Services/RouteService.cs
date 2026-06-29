using Contracts.Requests.Route;
using Contracts.Responses.Route;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IRouteService
    {
        Task<RouteResponse> GenerateAsync(RouteRequest request);
    }
    public class RouteService : IRouteService
    {
        private readonly IAccuWeatherService _weather;
        private readonly ITravelAssistantService _assistant;
        private readonly ITravelScoreService _travelScore;
        private readonly IRecommendationService _recommendation;

        public RouteService(IAccuWeatherService weather, ITravelAssistantService assistant, ITravelScoreService travelScore, IRecommendationService recommendation)
        {
            _weather = weather;
            _assistant = assistant;
            _travelScore = travelScore;
            _recommendation = recommendation;
        }

        public async Task<RouteResponse> GenerateAsync(RouteRequest request)
        {
            await Task.Delay(300);

            var weather = await _weather.GetCurrentWeatherAsync(request.Destination);
            var stops = _recommendation.GetStops(request);
            var score = _travelScore.Calculate(weather, request);

            var aiAdvice = await _assistant.GetTravelResponseAsync(
                        $"""
                        Route:
                        {request.Source} → {request.Destination}

                        Weather:
                        {weather.Condition}

                        Temperature:
                        {weather.TemperatureC}

                        Suggest:
                        Best departure time,
                        travel warnings,
                        rest advice.
                        """);

            return new RouteResponse
            {
                Source = request.Source,
                Destination = request.Destination,
                Distance = "280 km",
                Duration = "6h",
                Stops = stops,
                Weather = weather,
                TravelScore = score,
                Alerts = weather.IsTravelRecommended ? new() : new()
                    {
                        "High temperature expected"
                    },
                AiSuggestion = aiAdvice
            };
        }
    }
}