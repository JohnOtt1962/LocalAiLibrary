using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary
{
    public interface IAiManager
    {
        Task MakeAiCallAsync(List<ChatRequestMessage> conversationHistory, string userPrompt,
            string? toolUser = null, bool cacheChat = true);

        Task ProcessCategories();
    }
}