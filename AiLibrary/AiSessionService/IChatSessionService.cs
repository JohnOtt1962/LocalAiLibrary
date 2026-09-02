using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary.AiSessionService
{
    public interface IChatSessionService
    {
        List<ChatRequestMessage> GetHistory(string selectedCategoryId);
        void SaveHistory(List<ChatRequestMessage> history);
        List<ChatCategory> GetCategories();
        void SaveCategories(List<ChatCategory> categories);
        void ClearHistory();
    }
}