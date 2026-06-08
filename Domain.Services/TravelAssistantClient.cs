using Microsoft.Extensions.Configuration;

using OpenAI.Chat;

using System.ClientModel;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ITravelAssistantService
    {
        Task<string> GetTravelResponseAsync(string userInput);
    }

    public class TravelAssistantService : ITravelAssistantService
    {
        private readonly ChatClient _client;

        public TravelAssistantService(IConfiguration config)
        {
            var apiKey = config["OpenAI:ApiKey"];

            _client = new ChatClient(
                model: "gpt-4o-mini",
                credential: new ApiKeyCredential(apiKey)
            );
        }

        public async Task<string> GetTravelResponseAsync(string userInput)
        {
            var systemPrompt = @"
You are an expert Travel Assistant AI.
You help with:
- Trip planning
- Itineraries
- Budget travel
- Hotels & attractions
- Local recommendations

Always respond in structured format with clear sections.
";

            var response = await _client.CompleteChatAsync(
                new ChatMessage[]
                {
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userInput)
                }
            );

            return response.Value.Content[0].Text;
        }
    }
}