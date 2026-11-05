using LocalAiLibrary.AiLibrary.CategoryClassification.AiCategoryDbService;
using LocalAiLibrary.AiLibrary.CategoryClassification.AiCategoryService;
using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Utilities.Logger;

namespace LocalAiLibrary.AiLibrary.CategoryClassification
{
    public class ManageChatEntryClassificationService(IOptions<AiChatCategoryConfig> config, IAiChatCategoryRepo buildCategoryPrompt,
        IAiCategoryManager categoryManager, IUtilLogger logger) : IManageChatEntryClassificationService
    {
        private readonly AiChatCategoryConfig _config = config.Value;

        public async Task ProcessCategories()
        {
            List<DateItem> dates = await buildCategoryPrompt.GetChatDates();

            if (dates is { Count: > 0 })
            {
                AiCategorizationPayload payload = new AiCategorizationPayload
                {
                    CategoryPrompt = _config.CategoryPrompt,
                    Categories = await buildCategoryPrompt.GetChatCategories(),
                    UncategorizedChats = new List<UncategorizedChat>()
                };

                List<CategorizedChatEntry> categorizedEntries = await ProcessDatesAsync(dates, payload);

                foreach (CategorizedChatEntry item in categorizedEntries)
                {
                    await buildCategoryPrompt.SetChatCategory(item.ChatId, item.CategoryId);
                }
            }
        }

        private async Task<List<CategorizedChatEntry>> ProcessDatesAsync(List<DateItem> dates, AiCategorizationPayload payload)
        {
            List<CategorizedChatEntry> categorizedEntries = new List<CategorizedChatEntry>();
            
            foreach (DateItem dateItem in dates)
            {
                payload.UncategorizedChats = await buildCategoryPrompt.GetUncategorizedChatForDate(dateItem.DateFilter);
                string jsonPrompt = JsonSerializer.Serialize(payload);

                categorizedEntries.AddRange(await categoryManager.MakeAiCallAsync(jsonPrompt) ?? new List<CategorizedChatEntry>());
                await Task.Delay(TimeSpan.FromSeconds(2));
            }

            return categorizedEntries;
        }
    }
}