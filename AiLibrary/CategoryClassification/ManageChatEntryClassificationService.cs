using LocalAiLibrary.AiLibrary.CategoryClassification.AiCategoryDbService;
using LocalAiLibrary.AiLibrary.CategoryClassification.AiCategoryService;
using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LocalAiLibrary.AiLibrary.CategoryClassification
{
    public class ManageChatEntryClassificationService(IOptions<AiChatCategoryConfig> config, IAiChatCategoryRepo buildCategoryPrompt,
        IAiCategoryManager categoryManager) : IManageChatEntryClassificationService
    {
        private readonly AiChatCategoryConfig _config = config.Value;

        public async Task ProcessCategories()
        {
            List<DateItem> dates = buildCategoryPrompt.GetChatDates();

            AiCategorizationPayload payload = new AiCategorizationPayload
            {
                CategoryPrompt = _config.CategoryPrompt,
                Categories = buildCategoryPrompt.GetChatCategories(),
                UncategorizedChats = new List<UncategorizedChat>()
            };

            List<CategorizedChatEntry> categorizedEntries = await ProcessDatesAsync(dates, payload);

            foreach (CategorizedChatEntry item in categorizedEntries)
            {
                buildCategoryPrompt.SetChatCategory(item.ChatId, item.CategoryId);
            }

            string jsonPrompt = JsonSerializer.Serialize(categorizedEntries);
        }

        private async Task<List<CategorizedChatEntry>> ProcessDatesAsync(List<DateItem> dates, AiCategorizationPayload payload)
        {
            List<CategorizedChatEntry> categorizedEntries = new List<CategorizedChatEntry>();
            
            foreach (DateItem dateItem in dates)
            {
                payload.UncategorizedChats = buildCategoryPrompt.GetUncategorizedChatForDate(dateItem.DateFilter);
                string jsonPrompt = JsonSerializer.Serialize(payload);

                categorizedEntries.AddRange(await categoryManager.MakeAiCallAsync(jsonPrompt) ?? new List<CategorizedChatEntry>());
                await Task.Delay(TimeSpan.FromSeconds(2));
            }

            return categorizedEntries;
        }
    }
}