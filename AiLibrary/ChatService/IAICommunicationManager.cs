using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary.ChatService
{
    public interface IAiCommunicationManager
    {
        Task MakeAiCallAsync(List<ChatRequestMessage> ConversationHistory, string userPrompt, string? toolUser = null,
            bool cacheChat = true);
    }
}