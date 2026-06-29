using Microsoft.Extensions.Configuration;

using OpenAI.Chat;

using System;
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

            //if (string.IsNullOrWhiteSpace(apiKey))
            //{
            //    throw new InvalidOperationException("OpenAI API key not configured.");
            //}

            _client = new ChatClient(model: "gpt-4o-mini", credential: new ApiKeyCredential(apiKey));
        }

        public async Task<string> GetTravelResponseAsync(string userInput)
        {
            var systemPrompt =
"""
You are Humsafar AI Travel Companion.

Your job:
- Analyze weather
- Analyze route conditions
- Suggest ideal departure time
- Recommend stops
- Warn about difficult conditions
- Recommend essentials

Always respond EXACTLY in this format:

Travel Advice:
<short advice>

Warnings:
- item

Suggested Stops:
- item

Best Departure:
<time>

Keep responses practical and concise.
""";

            try
            {
                var response = await _client.CompleteChatAsync(
                            new ChatMessage[]
                            {
                                new SystemChatMessage(systemPrompt), new UserChatMessage(userInput)
                            });

                if (response?.Value?.Content?.Count > 0)
                {
                    return response.Value.Content[0].Text;
                }

                return "No recommendation available.";
            }
            catch (Exception ex)
            {
                return $"Travel assistant unavailable. {ex.Message}";
            }
        }
    }
}