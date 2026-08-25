using LocalAiLibrary.AiLibrary.AI_Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using Utilities.Repo;
using Utilities.Repo.Model;

namespace LocalAiLibrary.AiLibrary.AiDbHelper
{
    public class AiRepo(IOptions<AiConfig> config, IRepo repo) : IAiRepo
    {
        private readonly RepoHelper _repoHelper = new();
        private readonly AiConfig _config = config.Value;

        public List<ChatRequestMessage> GetConversationHistoryFromDb(string selectedCategoryId)
        {
            List<ChatRequestMessage> conversationHistory = new List<ChatRequestMessage>();

            if (conversationHistory.Count == 0)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                EntityOps ops = _repoHelper.GetEntityOps(_config.ProcGetDailyConversation, _config.ConnectionString);
                ops.Params = new List<ParamItem>
                {
                    _repoHelper.GetParamItem("ChatCategoryId", selectedCategoryId, false, "int", 1),
                };

                var collection = repo.GetCollection(ops);

                string jsonResponse = JsonSerializer.Serialize(collection);
                List<ChatRequestMessage> messages = JsonSerializer.Deserialize<List<ChatRequestMessage>>(jsonResponse, options);
                conversationHistory.AddRange(messages);
            }

            return conversationHistory;
        }

        public void InsertConversationFragment(string role, string content)
        {
            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcSaveConversation, _config.ConnectionString);
            ops.Params = new List<ParamItem>
            {
                _repoHelper.GetParamItem("Role", role, false, "varchar", 1),
                _repoHelper.GetParamItem("Content" , content, false, "varchar", 1)
            };

            int result = repo.Insert(ops);
        }

        public List<ChatCategory> GetChatCategories()
        {
            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcGetCategoryList, _config.ConnectionString);
            var chatCategoriesDictionaryList = repo.GetCollection(ops);
            string jsonCategories = JsonSerializer.Serialize(chatCategoriesDictionaryList);
            List<ChatCategory> chatCategories = JsonSerializer.Deserialize<List<ChatCategory>>(jsonCategories);
            return chatCategories;
        }
    }
}