using LocalAiLibrary.AiLibrary.AI_Models;
using LocalAiLibrary.AiLibrary.CategoryClassification.Models;

namespace LocalAiLibrary.AiLibrary.AI_ConversationHelper
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