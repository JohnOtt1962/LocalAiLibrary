using LocalAiLibrary.AiLibrary.CategoryClassification.Models;

namespace LocalAiLibrary.AiLibrary.CategoryClassification.AiCategoryDbService
{
    public interface IAiChatCategoryRepo
    {
        List<ChatCategory> GetChatCategories();
        List<DateItem> GetChatDates();
        List<UncategorizedChat> GetUncategorizedChatForDate(string date);
        int SetChatCategory(int chatId, int categoryId);
    }
}