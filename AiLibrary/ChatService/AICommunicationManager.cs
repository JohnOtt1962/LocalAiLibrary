using FluentValidation;
using LocalAiLibrary.AiLibrary.AI_Models.ModelValidation;
using LocalAiLibrary.AiLibrary.AiDbService;
using LocalAiLibrary.AiLibrary.AiSessionService;
using LocalAiLibrary.AiLibrary.AITools;
using LocalAiLibrary.AiLibrary.AITools.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Network;
using Microsoft.Extensions.Options;
using Utilities.Email;
using Utilities.Repo;

namespace LocalAiLibrary.AiLibrary.ChatService
{
    public class AiCommunicationManager(IChatSessionService chatSessionService, IAiNetwork aiNetwork, 
        IOptions<AiConfig> config, IEmail aiEmail, IValidator<ChatRequest> requestValidator, 
        IValidator<ChatCompletionResponse> responseValidator, IAiRepo aiRepo, IRepo repo) : IAiCommunicationManager
    {
        private readonly AiConfig _config = config.Value;
        private readonly ToolManager _toolManager = new(aiEmail, repo, config);

        public async Task MakeAiCallAsync(List<ChatRequestMessage> conversationHistory, string userPrompt,
            string? toolUser = null, bool cacheChat = true)
        {
            try
            {
                if (_config.EnableDbCaching && string.IsNullOrEmpty(toolUser))
                    aiRepo.InsertConversationFragment("user", userPrompt);

                await MakeAiCallAsyncExec(conversationHistory, userPrompt,
                    toolUser, cacheChat);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MakeAiCallAsync failed: {ex}");
                throw;
            }
        }

        private async Task MakeAiCallAsyncExec(List<ChatRequestMessage> conversationHistory, string userPrompt, string? toolUser = null, bool cacheChat = true)
        {
            if (cacheChat)
                conversationHistory.Add(new ChatRequestMessage("user", userPrompt, string.Empty));

            var request = ChatRequestBuilder.BuildAndValidate(requestValidator, _config.ModelName, _config.MaxTokens, conversationHistory);
            ChatCompletionResponse? response = await aiNetwork.GetResponse(_config.ApiKey, _config.Url, request);

            ManageResponse(conversationHistory, toolUser, response);

            chatSessionService.SaveHistory(conversationHistory);

            await ProcessToolCalls(response, conversationHistory);
        }

        private void ManageResponse(List<ChatRequestMessage> conversationHistory, string toolUser, ChatCompletionResponse? response)
        {
            ResponseReturnChecker.CheckValidation(response, responseValidator);

            if (!string.IsNullOrEmpty(toolUser))
                conversationHistory.RemoveAt(conversationHistory.Count - 1);

            if (response?.Choices is { Count: > 0 } choices && choices[0].Message != null && !string.IsNullOrEmpty(choices[0].Message.Content))
            {
                conversationHistory.Add(new ChatRequestMessage(choices[0].Message.Role, choices[0].Message.Content, string.Empty));

                if (_config.EnableDbCaching)
                    aiRepo.InsertConversationFragment(choices[0].Message.Role, choices[0].Message.Content);
            }
        }

        private async Task ProcessToolCalls(ChatCompletionResponse response, List<ChatRequestMessage> conversationHistory)
        {
            List<ToolCommand> commands = _toolManager.GetToolCommands(response);

            foreach (var command in commands)
            {
                string returnMessage = string.Empty;

                if (command.ToolName == "send_email")
                {
                    returnMessage = _toolManager.SendEmail(command);
                }

                if (command.ToolName == "get_person_address_schema")
                {
                    returnMessage = _toolManager.GetSchema(command);
                }

                if (command.ToolName == "read_database")
                {
                    returnMessage = _toolManager.ReadDatabase(command);
                }

                if (!string.IsNullOrEmpty(returnMessage))
                {
                    await MakeAiCallAsync(conversationHistory,
                        returnMessage,
                        "tool", true);
                }
            }
        }
    }
}