using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using LocalAiLibrary.AiLibrary.AiDbService;
using Utilities.Repo;
using Utilities.Repo.Model;

namespace LocalAiLibrary.AiLibrary.CategoryClassification.AiCategoryDbService
{
    public class AiChatCategoryRepo(IOptions<AiChatCategoryConfig> config, IRepo repo) : IAiChatCategoryRepo
    {
        private readonly DbOps _repoHelper = new();
        private readonly AiChatCategoryConfig _config = config.Value;

        public List<ChatCategory> GetChatCategories()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcGetChatCategories, _config.ConnectionString);
            var result = repo.GetCollection(ops);
            string jsonResponse = JsonSerializer.Serialize(result);
            List<ChatCategory> categories = JsonSerializer.Deserialize<List<ChatCategory>>(jsonResponse, options);

            return categories;
        }

        public List<DateItem> GetChatDates()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcGetUncategorizedChatDates, _config.ConnectionString);
            var result = repo.GetCollection(ops);
            string jsonResponse = JsonSerializer.Serialize(result);
            List<DateItem> dateList = JsonSerializer.Deserialize<List<DateItem>>(jsonResponse, options);

            return dateList;
        }

        public List<UncategorizedChat> GetUncategorizedChatForDate(string date)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcGetUncategorizedChatByDate, _config.ConnectionString);
            ops.Params = new List<ParamItem>
            {
                _repoHelper.GetParamItem("DateFilter", date, false, "varchar", 1),
            };

            var result = repo.GetCollection(ops);
            string jsonResponse = JsonSerializer.Serialize(result);
            List<UncategorizedChat> chatItems = JsonSerializer.Deserialize<List<UncategorizedChat>>(jsonResponse, options);

            return chatItems;
        }

        public int SetChatCategory(int chatId, int categoryId)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcSetChatCategory, _config.ConnectionString);
            ops.Params = new List<ParamItem>
            {
                _repoHelper.GetParamItem("Id", Convert.ToString(chatId), false, "int", 1),
                _repoHelper.GetParamItem("CategoryId", Convert.ToString(categoryId), false, "int", 1)
            };

            var result = repo.Insert(ops);

            return result;
        }
    }
}