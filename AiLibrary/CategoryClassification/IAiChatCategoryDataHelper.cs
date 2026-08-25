using LocalAiLibrary.AiLibrary.CategoryClassification.Models;

namespace LocalAiLibrary.AiLibrary.CategoryClassification
{
    public interface IAiChatCategoryDataHelper
    {
        List<ChatCategory> GetChatCategories();
        List<DateItem> GetChatDates();
        List<UncategorizedChat> GetUncategorizedChatForDate(string date);
        int SetChatCategory(int chatId, int categoryId);
    }
}