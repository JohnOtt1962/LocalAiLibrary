using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Orchestration.Models;

namespace LocalAiLibrary.AiLibrary.Orchestration
{
    public interface IOrchestrationPlanManagement
    {
        OrchestrationPlan? SaveOrchestrationPlan(ChatCompletionResponse? response);
        string ProcessOrchestration(OrchestrationPlan orchestrationPlan);
        string CacheToolResponse(string returnMessage, bool isDataCall);
        string GetOrchestrationPrompt();
    }
}