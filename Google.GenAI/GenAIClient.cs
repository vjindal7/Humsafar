using Google.GenAI;
using Google.GenAI.Types;

using System.Text.RegularExpressions;

namespace GoogleGenAI
{
    public interface IGenAIClient
    {
        Task<PromptResponse> GenerateResponse(PromptRequest request);
    }

    public class GenAIClient : IGenAIClient
    {
        private readonly string apiKey;
        private readonly string model;
        public GenAIClient()
        {
            apiKey = "";
            model = "gemini-2.5-flash";
        }

        public async Task<PromptResponse> GenerateResponse(PromptRequest request)
        {
            var systemInstruction = new Content
            {
                Parts =
                [
                    new Part { Text = request.SystemMessage }
                ]
            };

            var client = new Client(apiKey: apiKey);

            var response = await client.Models.GenerateContentAsync(
                model: model,
                contents: request.UserInput,
                config: new GenerateContentConfig
                {
                    SystemInstruction = systemInstruction
                });

            var json = Regex.Replace(response.Text, @"^```json\s*|\s*```$", "", RegexOptions.Multiline);
            return new PromptResponse() { Response = json };
        }
    }
}
