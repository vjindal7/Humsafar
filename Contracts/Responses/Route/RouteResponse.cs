using Contracts.Responses.Weather;
using System.Collections.Generic;

namespace Contracts.Responses.Route
{
    public class RouteResponse
    {
        public string Source { get; set; }

        public string Destination { get; set; }

        public string Distance { get; set; }

        public string Duration { get; set; }

        public string BestDepartureTime { get; set; }

        public List<StopResponse> Stops { get; set; }

        public WeatherResponse Weather { get; set; }

        public List<string> Alerts { get; set; }

        public string AiSuggestion { get; set; }

        public bool WeatherRecommended { get; set; }

        public string AlternativeRoute { get; set; }

        public int TravelScore { get; set; }
    }

    public class StopResponse
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }
    }
}
