using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Helpers;
using LocalAiLibrary.AiLibrary.Orchestration.Models;
using Microsoft.AspNetCore.Session;
using System.Text.Json;

namespace LocalAiLibrary.AiLibrary.AiSessionService
{
    public class StaticSessionService : IChatSessionService
    {
        private static string _workingOrchestration;
        private static string _serviceName;

        public void SaveOrchestration(OrchestrationPlan orchestrationPlan)
        {
            _workingOrchestration = JsonSerializer.Serialize(orchestrationPlan);
        }

        public string GetServiceName()
        {
            return _serviceName;
        }

        public void SetServiceName(string serviceName)
        {
            _serviceName = serviceName;
        }

        public OrchestrationPlan? GetOrchestration()
        {
            return DeserializerHelper.DeserializeItem<OrchestrationPlan>(_workingOrchestration);
        }

        public Task<List<ChatRequestMessage>> GetHistory(string selectedCategoryId)
        {
            return null!;
            //throw new NotImplementedException();
        }

        public void SaveHistory(List<ChatRequestMessage> history)
        {
            //  no action
            //throw new NotImplementedException();
        }

        public Task<List<ChatCategory>?> GetCategories()
        {
            return null!;
            //throw new NotImplementedException();
        }

        public Task<List<ChatApplication>?> GetApplications()
        {
            return null!;
            //throw new NotImplementedException();
        }

        public void SetApplicationName(int applicationId)
        {
            //  no action 
        }

        public void SaveCategories(List<ChatCategory> categories)
        {
            //  no action
            //throw new NotImplementedException();
        }

        public void ClearHistory()
        {
            //  no action
            //throw new NotImplementedException();
        }

        public void ClearOrchestration()
        {
            _workingOrchestration = string.Empty;
        }
    }
}