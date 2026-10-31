using System.Text.Json;
using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Helpers;
using Microsoft.Extensions.Options;
using Utilities.Repo;
using Utilities.Repo.Model;

namespace LocalAiLibrary.AiLibrary.AiDbService
{
    internal class AiRepo(IOptions<AiConfig> config, IRepo repo) : IAiRepo
    {
        private readonly DbOps _repoHelper = new();
        private readonly AiConfig _config = config.Value;

        public async Task<List<ChatRequestMessage>> GetConversationHistoryFromDb(string selectedCategoryId)
        {
            List<ChatRequestMessage> conversationHistory = new List<ChatRequestMessage>();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcGetDailyConversation, _config.ConnectionString);
            ops.Params = new List<ParamItem>
            {
                _repoHelper.GetParamItem("ChatCategoryId", selectedCategoryId, false, "int", 1),
            };

            var collection = await repo.GetCollection(ops);

            string jsonResponse = JsonSerializer.Serialize(collection);
            List<ChatRequestMessage>? messages = DeserializerHelper.DeserializeItem<List<ChatRequestMessage>>(jsonResponse, options);

            if(messages != null)
                conversationHistory.AddRange(messages);

            return conversationHistory;
        }

        public async Task InsertConversationFragment(string role, string content)
        {
            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcSaveConversation, _config.ConnectionString);
            ops.Params = new List<ParamItem>
            {
                _repoHelper.GetParamItem("Role", role, false, "varchar", 1),
                _repoHelper.GetParamItem("Content" , content, false, "varchar", -1)
            };

            int result = await repo.Insert(ops);
        }

        public async Task RemoveConversationFragment()
        {
            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcRemoveConversation, _config.ConnectionString);
            await repo.Delete(ops);
        }

        public async Task<List<ChatCategory>?> GetChatCategories()
        {
            List<ChatCategory>? chatCategories = new List<ChatCategory>();
            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcGetCategoryList, _config.ConnectionString);
            var chatCategoriesDictionaryList = await repo.GetCollection(ops);

            string jsonCategories = JsonSerializer.Serialize(chatCategoriesDictionaryList);
            chatCategories = DeserializerHelper.DeserializeItem<List<ChatCategory>>(jsonCategories);

            return chatCategories;
        }

        public async Task InsertGemmaEmailChatItem(string participant, string content, string role)
        {
            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcInsertGemmaEmailChat, _config.ConnectionString);
            ops.Params = new List<ParamItem>
            {
                _repoHelper.GetParamItem("Role", role, false, "varchar", -1),
                _repoHelper.GetParamItem("Content" , content, false, "varchar", -1),
                _repoHelper.GetParamItem("Participant" , participant, false, "varchar", -1)
            };

            await repo.Insert(ops);
        }

        public async Task<List<ChatRequestMessage>> GetGemmaEmailChatItem(string participant)
        {
            List<ChatRequestMessage> conversationHistory = new List<ChatRequestMessage>();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcGetGemmaEmailChat, _config.ConnectionString);
            ops.Params = new List<ParamItem>
            {
                _repoHelper.GetParamItem("Participant" , participant, false, "varchar", -1)
            };

            var collection = await repo.GetCollection(ops);

            string jsonResponse = JsonSerializer.Serialize(collection);
            List<ChatRequestMessage>? messages = DeserializerHelper.DeserializeItem<List<ChatRequestMessage>>(jsonResponse, options);

            if (messages != null)
                conversationHistory.AddRange(messages);

            return conversationHistory;

        }
    }
}