using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using Microsoft.Extensions.Options;
using Utilities.Logger;
using Utilities.Repo;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace LocalAiLibrary.AiLibrary.AiDbService
{
    public class AiRepoManager(IOptions<AiConfig> config, IRepo repo, IUtilLogger logger) : IAiRepo
    {
        private readonly AiRepo _aiRepo = new(config, repo);
        private readonly AiConfig _config = config.Value;

        public async Task<List<ChatRequestMessage>> GetConversationHistoryFromDb(string selectedCategoryId)
        {
            List<ChatRequestMessage> conversationHistory = new List<ChatRequestMessage>();

            try
            {
                conversationHistory = await _aiRepo.GetConversationHistoryFromDb(selectedCategoryId);
            }
            catch (Exception ex)
            {
                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }

            return conversationHistory;
        }

        public async Task InsertConversationFragment(string role, string content)
        {
            try
            {
                await _aiRepo.InsertConversationFragment(role, content);
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

        public async Task RemoveConversationFragment()
        {
            try
            {
                await _aiRepo.RemoveConversationFragment();
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

        public async Task<List<ChatCategory>?> GetChatCategories()
        {
            List<ChatCategory>? chatCategories = new List<ChatCategory>();

            try
            {
                chatCategories = await _aiRepo.GetChatCategories();
            }
            catch (Exception ex)
            {
                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }

            return chatCategories;
        }

        public async Task<List<ChatRequestMessage>> GetGemmaEmailChatItem(string participant)
        {
            List<ChatRequestMessage> conversationHistory = new List<ChatRequestMessage>();

            try
            {
                conversationHistory = await _aiRepo.GetGemmaEmailChatItem(participant);
            }
            catch (Exception ex)
            {
                _ = Task.Run(async () =>
                {
                    await logger.Log(ex.Message, ex, GetType().Name, LoggerBase.LogLevel.Warning,
                        new ConnectionDetails { ConnectionString = _config.ConnectionString, ProcedureName = _config.LoggerProcedure });
                });
            }

            return conversationHistory;
        }

        public async Task InsertGemmaEmailChatItem(string participant, string role, string content)
        {
            try
            {
                await _aiRepo.InsertGemmaEmailChatItem(participant, role, content);
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
    }
}
