using Contracts.Requests.Route;
using Contracts.Responses.Weather;
using System;

namespace Domain.Services
{
    public interface ITravelScoreService
    {
        int Calculate(WeatherResponse weather, RouteRequest request);
    }

    public class TravelScoreService : ITravelScoreService
    {
        public int Calculate(WeatherResponse weather, RouteRequest request)
        {
            var score = 100;

            if (!weather.IsTravelRecommended)
                score -= 30;

            if (request.Budget < 5000)
                score -= 10;

            if (request.ScenicRoute)
                score += 10;

            return Math.Max(0, score);
        }
    }
}
