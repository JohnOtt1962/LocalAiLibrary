using FluentValidation;
using LocalAiLibrary.AiLibrary.AI_Models.ModelValidation;
using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Network;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LocalAiLibrary.AiLibrary.CategoryClassification.AiCategoryService
{
    public class AiCategoryManager(IOptions<AiConfig> config, IValidator<ChatRequest> requestValidator,
        IValidator<ChatCompletionResponse> responseValidator, IAiNetwork aiNetwork) : IAiCategoryManager
    {
        private readonly AiConfig _config = config.Value;

        public async Task<List<CategorizedChatEntry>?> MakeAiCallAsync(string jsonPrompt)
        {
            try
            {
                return await MakeAiCallAsyncExec(jsonPrompt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MakeAiCallAsync failed: {ex}");
                throw;
            }

            return null;
        }

        private async Task<List<CategorizedChatEntry>> MakeAiCallAsyncExec(string jsonPrompt)
        {
            List<CategorizedChatEntry> categorizedEntries = [];

            List<ChatRequestMessage> conversationHistory =
            [
                new ChatRequestMessage("user", jsonPrompt, string.Empty)
            ];
                
            var request = ChatRequestBuilder.BuildAndValidate(requestValidator, _config.ModelName, _config.MaxTokens, conversationHistory);
            ChatCompletionResponse? response = await aiNetwork.GetResponse(_config.ApiKey, _config.Url, request);

            ResponseReturnChecker.CheckValidation(response, responseValidator);

            try
            {
                categorizedEntries =
                    JsonSerializer.Deserialize<List<CategorizedChatEntry>>(response.Choices[0].Message.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deserialization failed: {ex}");
                throw;
            }

            return categorizedEntries;
        }
    }
}