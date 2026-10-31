using LocalAiLibrary.AiLibrary.CategoryClassification.Models;

namespace LocalAiLibrary.AiLibrary.CategoryClassification.AiCategoryDbService
{
    public interface IAiChatCategoryRepo
    {
        Task<List<ChatCategory>> GetChatCategories();
        Task<List<DateItem>> GetChatDates();
        Task<List<UncategorizedChat>> GetUncategorizedChatForDate(string date);
        Task<int> SetChatCategory(int chatId, int categoryId);
    }
}