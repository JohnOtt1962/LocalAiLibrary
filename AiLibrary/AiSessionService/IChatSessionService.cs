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
        Task<List<ChatApplication>?> GetApplications();
        void SaveCategories(List<ChatCategory> categories);
        void ClearHistory();
        void ClearOrchestration();
        void SaveOrchestration(OrchestrationPlan orchestrationPlan);
        OrchestrationPlan? GetOrchestration();
        string GetServiceName();
        void SetServiceName(string serviceName);
        void SetApplicationName(int applicationId);
    }
}