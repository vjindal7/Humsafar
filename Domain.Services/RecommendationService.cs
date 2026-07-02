using Contracts.Requests.Route;
using Contracts.Responses.Route;

using GoogleGenAI;

using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IRecommendationService
    {
        Task<List<StopResponse>> GetStops(RouteRequest request);
    }
    public class RecommendationService : IRecommendationService
    {
        private readonly IGenAIClient _genAIClient;
        public RecommendationService(IGenAIClient genAIClient)
        {
            _genAIClient = genAIClient;
        }

        public async Task<List<StopResponse>> GetStops(RouteRequest request)
        {
            var req = new PromptRequest()
            {
                SystemMessage = """
You are a travel assitant who recommends the places to visit along with good food outlets in range of 50Km based on below input 
-latitude
-longitude
-tripType (Roadtrip, Weekend, Long Trip)
-travelers (Solo, Couple, Family, Friends)
-Vibe (Relaxed, Adventure, Cultural, Party)
-Budget (High, Medium, Low)

Output must be list of places and contain 1 places for food outlet and 2 for best places to visit

Output should be json and includes below fields
-PlaceName
-PlaceType (Food, Scenic)
-Description
-LatOffset
-LonOffset
-Rating
""",
                UserInput = $"""
latitude-{request.CurrentLatitude}
longitude-{request.CurrentLongitude}
tripType-{request.TripType}
travelers-{request.Travelers}
vibe-{request.Vibe}
budget-{request.Budget}
"""
            };

            var genAIResponse = await _genAIClient.GenerateResponse(req);
            var response = JsonSerializer.Deserialize<List<StopResponse>>(genAIResponse.Response);
            return response;
        }
    }
}
