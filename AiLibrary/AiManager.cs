using LocalAiLibrary.AiLibrary.CategoryClassification;
using LocalAiLibrary.AiLibrary.ChatService;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.EmailChatService;
using Microsoft.Extensions.Options;
using Utilities.Logger;

namespace LocalAiLibrary.AiLibrary
{
    public class AiManager(IAiCommunicationManager aiCommunicationManager, IManageChatEntryClassificationService aiCategoryService,
        IOutstandingChatEmails outstandingChatEmails, IUtilLogger logger, IOptions<AiConfig> config) : IAiManager
    {
        private readonly AiConfig _config = config.Value;

        public async Task MakeAiCallAsync(List<ChatRequestMessage> conversationHistory, string userPrompt)
        {
            try
            {
                await aiCommunicationManager.MakeAiCallAsync(conversationHistory, userPrompt);
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

        public async Task ProcessCategories()
        {
            try
            {
                await aiCategoryService.ProcessCategories();
            }
            catch (Exception ex)
            {
                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Debug,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }
        }

        public async Task ProcessChatEmails()
        {
            try
            {
                await outstandingChatEmails.GetOutstandingChatEmails();
            }
            catch (Exception ex)
            {
                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Debug,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }
        }
    }
}