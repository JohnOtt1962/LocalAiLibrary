using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Orchestration.Models;

namespace LocalAiLibrary.AiLibrary.AiSessionService
{
    public interface IChatSessionService
    {
        Task<List<ChatRequestMessage>> GetHistory(string selectedCategoryId);
        void SaveHistory(List<ChatRequestMessage> history);
        Task<List<ChatCategory>?> GetCategories();
        void SaveCategories(List<ChatCategory> categories);
        void ClearHistory();
        void SaveOrchestration(OrchestrationPlan orchestrationPlan);
        OrchestrationPlan? GetOrchestration();
        void ClearOrchestration();
    }
}