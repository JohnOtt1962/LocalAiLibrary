using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary.AiDbService
{
    public interface IAiRepo
    {
        List<ChatRequestMessage> GetConversationHistoryFromDb(string selectedCategoryId);
        List<ChatCategory> GetChatCategories();
        void InsertConversationFragment(string role, string content);
    }
}