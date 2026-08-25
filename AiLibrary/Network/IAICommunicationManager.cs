using LocalAiLibrary.AiLibrary.AI_Models;
using LocalAiLibrary.AiLibrary.CategoryClassification.Models;

namespace LocalAiLibrary.AiLibrary.Network
{
    public interface IAiCommunicationManager
    {
        Task MakeAiCallAsync(List<ChatRequestMessage> ConversationHistory, string userPrompt, string? toolUser = null,
            bool cacheChat = true);
    }
}