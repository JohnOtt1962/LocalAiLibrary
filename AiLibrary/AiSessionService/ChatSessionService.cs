using LocalAiLibrary.AiLibrary.AiDbService;
using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using LocalAiLibrary.AiLibrary.ChatService.Models;
using LocalAiLibrary.AiLibrary.Helpers;
using LocalAiLibrary.AiLibrary.Orchestration.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LocalAiLibrary.AiLibrary.AiSessionService;

public class ChatSessionService(IAiRepo aiRepo, IOptions<AiConfig> config, ISessionStore sessionStore)
    : IChatSessionService
{
    private const string SessionKey = "ChatHistory";
    private const string SessionCategoriesKey = "SessionCategories";
    private const string OrchestrationPlanKey = "OrchestrationPlanKey";
    private readonly AiConfig _config = config.Value;
    private readonly DbOps _repoHelper = new();

    public void SaveOrchestration(OrchestrationPlan orchestrationPlan)
    {
        var json = JsonSerializer.Serialize(orchestrationPlan);
        sessionStore.SaveObject(OrchestrationPlanKey, json);
    }

    public OrchestrationPlan? GetOrchestration()
    {
        OrchestrationPlan? orchestrationPlan = null;
        var json = sessionStore.GetObject(OrchestrationPlanKey);

        if (!string.IsNullOrEmpty(json))
            orchestrationPlan = DeserializerHelper.DeserializeItem<OrchestrationPlan>(json);

        return orchestrationPlan;
    }

    public void ClearOrchestration()
    {
        sessionStore.ClearObject(OrchestrationPlanKey);
    }

    public async Task<List<ChatRequestMessage>> GetHistory(string selectedCategoryId)
    {
        var json = sessionStore.GetObject(SessionKey);
        List<ChatRequestMessage>? messages = new List<ChatRequestMessage>();

        if (string.IsNullOrEmpty(json))
        {
            if (_config.EnableDbCaching)
                messages = await aiRepo.GetConversationHistoryFromDb(selectedCategoryId);
        }
        else
        {
            messages = DeserializerHelper.DeserializeItem<List<ChatRequestMessage>>(json)!;
        }

        return messages;
    }

    public void SaveHistory(List<ChatRequestMessage> history)
    {
        var json = JsonSerializer.Serialize(history);
        sessionStore.SaveObject(SessionKey, json);
    }

    public async Task<List<ChatCategory>?> GetCategories()
    {
        var json = sessionStore.GetObject(SessionCategoriesKey);
        List<ChatCategory>? categories = new List<ChatCategory>();

        if (string.IsNullOrEmpty(json))
        {
            if (_config.EnableDbCaching)
            {
                categories = await aiRepo.GetChatCategories();
                if (categories != null) SaveCategories(categories);
            }
        }
        else
        {
            categories = DeserializerHelper.DeserializeItem<List<ChatCategory>>(json);
        }

        return categories;
    }

    public void SaveCategories(List<ChatCategory> categories)
    {
        var json = JsonSerializer.Serialize(categories);
        sessionStore.SaveObject(SessionCategoriesKey, json);
    }

    public void ClearHistory()
    {
        sessionStore.ClearObject(SessionKey);
    }
}