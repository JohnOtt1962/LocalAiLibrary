using LocalAiLibrary.AiLibrary.AI_Models;
using LocalAiLibrary.AiLibrary.CategoryClassification.Models;

namespace LocalAiLibrary.AiLibrary.AiDbHelper
{
    public interface IAiRepo
    {
        List<ChatRequestMessage> GetConversationHistoryFromDb(string selectedCategoryId);
        List<ChatCategory> GetChatCategories();
        void InsertConversationFragment(string role, string content);
    }
}