using FluentValidation;
using LocalAiLibrary.AiLibrary.AI_Models.ModelValidation;
using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Helpers;
using LocalAiLibrary.AiLibrary.Network;
using LocalAiLibrary.AiLibrary.Validation.ModelValidation;
using Microsoft.Extensions.Options;
using Utilities.Logger;

namespace LocalAiLibrary.AiLibrary.CategoryClassification.AiCategoryService
{
    public class AiCategoryManager(IOptions<AiConfig> config, IValidator<ChatRequest> requestValidator,
        IValidator<ChatCompletionResponse> responseValidator, IAiNetwork aiNetwork, IUtilLogger logger) : IAiCategoryManager
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
                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });

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
                
            var request = ChatRequestBuilder.BuildAndValidate(requestValidator, _config.ModelName, _config.MaxTokens, conversationHistory, ChatRequestBuilder.ToolOperation.IncludeNoTools);
            ChatCompletionResponse? response = await aiNetwork.GetResponse(_config.ApiKey, _config.Url, request);

            ResponseReturnChecker.CheckValidation(response, responseValidator);

            categorizedEntries =
                DeserializerHelper.DeserializeItem<List<CategorizedChatEntry>>(response.Choices[0].Message.Content)!;

            if (categorizedEntries == null)
                categorizedEntries = new List<CategorizedChatEntry>();

            return categorizedEntries;
        }
    }
}