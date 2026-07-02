using Contracts.Responses.Weather;

using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("PlaceName")]
        public string PlaceName { get; set; } = string.Empty;

        [JsonPropertyName("PlaceType")]
        public string PlaceType { get; set; } = string.Empty;

        [JsonPropertyName("Description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("LatOffset")]
        public double LatOffset { get; set; }

        [JsonPropertyName("LonOffset")]
        public double LonOffset { get; set; }

        [JsonPropertyName("Rating")]
        public double Rating { get; set; }
    }
}
