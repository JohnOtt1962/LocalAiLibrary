using LocalAiLibrary.AiLibrary.AiDbService;
using LocalAiLibrary.AiLibrary.AiSessionService;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Helpers;
using LocalAiLibrary.AiLibrary.Orchestration.Models;
using Microsoft.Extensions.Options;
using Utilities.Logger;

namespace LocalAiLibrary.AiLibrary.ChatService
{
    public class PromptHelper(IOptions<AiConfig> config, IAiRepo aiRepo, IChatSessionService chatSessionService,
        IUtilLogger logger) : IPromptHelper
    {
        private readonly AiConfig _config = config.Value;

        public string GetPromptObject(string userPrompt)
        {
            string returnPrompt = userPrompt;

            try
            {
                DataChat? dataChat = DeserializerHelper.DeserializeItem<DataChat>(userPrompt);

                if (dataChat != null)
                    returnPrompt = dataChat.Prompt;
            }
            catch (Exception ex)
            {
                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }

            return returnPrompt;
        }

        public async Task InsertConversationFragment(string role, string userPrompt)
        {
            try
            {
                if (_config.EnableDbCaching)
                    await aiRepo.InsertConversationFragment(role, userPrompt);
            }
            catch (Exception ex)
            {
                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }
        }

        public async Task AddOrchestrationPromptFragment(string userPrompt)
        {
            string conversationPrompt = GetPromptObject(userPrompt);
            
            if (_config.EnableDbCaching && !string.IsNullOrEmpty(conversationPrompt))
                await InsertConversationFragment("user", conversationPrompt);
        }

        public void AddPromptToActiveContext(List<ChatRequestMessage> conversationHistory, string role, string userPrompt, string toolCallId)
        {
            conversationHistory.Add(new ChatRequestMessage(role, userPrompt, toolCallId));
        }

        public void RemovePromptFromActiveContext(List<ChatRequestMessage> conversationHistory)
        {
            conversationHistory.RemoveAt(conversationHistory.Count - 1);
            chatSessionService.SaveHistory(conversationHistory);
        }

        public async Task AddAssistantToConversation(List<ChatRequestMessage> conversationHistory, IReadOnlyList<ChatChoice> choices)
        {
            AddPromptToActiveContext(conversationHistory, choices[0].Message.Role, choices[0].Message.Content, string.Empty);

            if (_config.EnableDbCaching)
                await InsertConversationFragment(choices[0].Message.Role, choices[0].Message.Content);
        }
    }
}