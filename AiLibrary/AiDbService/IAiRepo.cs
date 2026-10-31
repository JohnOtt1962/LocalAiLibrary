using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary.AiDbService
{
    public interface IAiRepo
    {
        Task<List<ChatRequestMessage>> GetConversationHistoryFromDb(string selectedCategoryId);
        Task<List<ChatCategory>?> GetChatCategories();
        Task InsertConversationFragment(string role, string content);
        Task RemoveConversationFragment();
        Task<List<ChatRequestMessage>> GetGemmaEmailChatItem(string participant);
        Task InsertGemmaEmailChatItem(string participant, string content, string role);
    }
}