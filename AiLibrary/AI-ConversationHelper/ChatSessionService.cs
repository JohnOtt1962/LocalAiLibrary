using LocalAiLibrary.AiLibrary.AI_Models;
using LocalAiLibrary.AiLibrary.AiDbHelper;
using LocalAiLibrary.AiLibrary.CategoryClassification.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LocalAiLibrary.AiLibrary.AI_ConversationHelper;

public class ChatSessionService(IHttpContextAccessor httpContextAccessor, IAiRepo aiRepo, IOptions<AiConfig> config)
    : IChatSessionService
{
    private const string SessionKey = "ChatHistory";
    private const string SessionCategoriesKey = "SessionCategories";
    private readonly AiConfig _config = config.Value;
    private readonly RepoHelper _repoHelper = new();

    private ISession Session => httpContextAccessor.HttpContext!.Session;

    public List<ChatRequestMessage> GetHistory(string selectedCategoryId)
    {
        var json = Session.GetString(SessionKey);
        List<ChatRequestMessage> messages = new List<ChatRequestMessage>();

        if (string.IsNullOrEmpty(json))
        {
            if (_config.EnableDbCaching)
                messages = aiRepo.GetConversationHistoryFromDb(selectedCategoryId);
        }
        else
        {
            messages = JsonSerializer.Deserialize<List<ChatRequestMessage>>(json)!;
        }

        return messages;
    }

    public void SaveHistory(List<ChatRequestMessage> history)
    {
        var json = JsonSerializer.Serialize(history);
        Session.SetString(SessionKey, json);
    }

    public List<ChatCategory> GetCategories()
    {
        var json = Session.GetString(SessionCategoriesKey);
        List<ChatCategory>? categories = new List<ChatCategory>();

        if (string.IsNullOrEmpty(json))
        {
            if (_config.EnableDbCaching)
            {
                categories = aiRepo.GetChatCategories();
                SaveCategories(categories);
            }
        }
        else
        {
            categories = JsonSerializer.Deserialize<List<ChatCategory>>(json);
        }

        return categories;
    }

    public void SaveCategories(List<ChatCategory> categories)
    {
        var json = JsonSerializer.Serialize(categories);
        Session.SetString(SessionCategoriesKey, json);
    }

    public void ClearHistory()
    {
        Session.Remove(SessionKey);
    }
}