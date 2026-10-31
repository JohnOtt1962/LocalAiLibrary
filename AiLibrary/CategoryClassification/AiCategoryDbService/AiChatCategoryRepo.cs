using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using LocalAiLibrary.AiLibrary.AiDbService;
using LocalAiLibrary.AiLibrary.Helpers;
using Utilities.Repo;
using Utilities.Repo.Model;

namespace LocalAiLibrary.AiLibrary.CategoryClassification.AiCategoryDbService
{
    public class AiChatCategoryRepo(IOptions<AiChatCategoryConfig> config, IRepo repo) : IAiChatCategoryRepo
    {
        private readonly DbOps _repoHelper = new();
        private readonly AiChatCategoryConfig _config = config.Value;

        public async Task<List<ChatCategory>> GetChatCategories()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcGetChatCategories, _config.ConnectionString);
            var result = await repo.GetCollection(ops);
            string jsonResponse = JsonSerializer.Serialize(result);
            List<ChatCategory> categories = DeserializerHelper.DeserializeItem<List<ChatCategory>>(jsonResponse, options)!;

            if (categories == null)
                categories = new List<ChatCategory>();

            return categories;
        }

        public async Task<List<DateItem>> GetChatDates()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EntityOps ops = _repoHelper.GetEntityOps(_config.ProcGetUncategorizedChatDates, _config.ConnectionString);
            var result = await repo.GetCollection(ops);
            string jsonResponse = JsonSerializer.Serialize(result);
            List<DateItem> dateList = DeserializerHelper.DeserializeItem<List<DateItem>>(jsonResponse, options)!;

            if (dateList == null)
                dateList = new List<DateItem>();

            return dateList;
        }

        public async Task<List<UncategorizedChat>> GetUncategorizedChatForDate(string date)
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

            var result = await repo.GetCollection(ops);
            string jsonResponse = JsonSerializer.Serialize(result);
            List<UncategorizedChat> chatItems = DeserializerHelper.DeserializeItem<List<UncategorizedChat>>(jsonResponse, options)!;

            if(chatItems == null)
                chatItems = new List<UncategorizedChat>();

            return chatItems;
        }

        public async Task<int> SetChatCategory(int chatId, int categoryId)
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

            var result = await repo.Insert(ops);

            return result;
        }
    }
}