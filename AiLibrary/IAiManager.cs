using LocalAiLibrary.AiLibrary.ChatService.Models;

namespace LocalAiLibrary.AiLibrary
{
    public interface IAiManager
    {
        Task MakeAiCallAsync(List<ChatRequestMessage> conversationHistory, string userPrompt);
        Task ProcessCategories();
        Task ProcessChatEmails();
    }
}